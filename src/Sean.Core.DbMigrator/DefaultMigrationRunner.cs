using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Sean.Core.DbMigrator;

public class DefaultMigrationRunner : IMigrationRunner
{
    private readonly IMigrationFactory _migrationFactory;
    private readonly MigrationOptions _migrationOptions;

    public DefaultMigrationRunner(IMigrationFactory migrationFactory, MigrationOptions migrationOptions)
    {
        _migrationFactory = migrationFactory ?? throw new ArgumentNullException(nameof(migrationFactory));
        _migrationOptions = migrationOptions ?? throw new ArgumentNullException(nameof(migrationOptions));
    }

    public void Upgrade()
    {
        Upgrade(long.MaxValue);
    }

    public void Upgrade(long targetVersion)
    {
        var currentVersion = _migrationFactory.GetCurrentVersion();
        if (currentVersion >= targetVersion) return;

        var timeRecord = new Stopwatch();
        if (_migrationOptions.Assemblies != null)
        {
            var migrationInfos = new List<MigrationInfo>();

            #region 1. Search migration target.
            foreach (var assembly in _migrationOptions.Assemblies)
            {
                var types = assembly.GetTypes().Where(migrationClassType => migrationClassType.IsClass && !migrationClassType.IsAbstract && typeof(Migration).IsAssignableFrom(migrationClassType) && (_migrationOptions.MigrationFilter == null || _migrationOptions.MigrationFilter(migrationClassType))).ToList();
                types.ForEach(type =>
                {
                    var migrationAttribute = type.GetCustomAttribute<MigrationAttribute>(true);
                    if (migrationAttribute == null)
                    {
                        return;
                    }

                    var version = migrationAttribute.Version;
                    if (version > currentVersion && version <= targetVersion)
                    {
                        migrationInfos.Add(new MigrationInfo
                        {
                            Version = version,
                            Description = migrationAttribute.Description,
                            MigrationClassType = type,
                            Assembly = assembly
                        });
                    }
                });
            }

            if (_migrationOptions.AutoMigrateFromFiles)
            {
                _migrationOptions.ScriptOptions ??= new MigrationScriptOptions();

                var migrationPrefix = _migrationOptions.ScriptOptions.MigrationPrefix;
                var migrationSeparator = _migrationOptions.ScriptOptions.MigrationSeparator;
                var migrationScriptExtension = _migrationOptions.ScriptOptions.MigrationScriptExtension;

                if (!_migrationOptions.ScriptOptions.EmbeddedScript)
                {
                    var scriptFilePaths = Directory.GetFiles(_migrationOptions.ScriptBaseDirectory, $"{migrationPrefix}*{migrationSeparator}*{migrationScriptExtension}", SearchOption.TopDirectoryOnly);
                    if (scriptFilePaths.Any())
                    {
                        foreach (var scriptFilePath in scriptFilePaths)
                        {
                            var fileName = Path.GetFileName(scriptFilePath);
                            //var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(scriptFilePath);
                            //var split = fileNameWithoutExtension.Split(new[] { migrationSeparator }, StringSplitOptions.RemoveEmptyEntries);
                            if (string.IsNullOrEmpty(fileName))
                            {
                                continue;
                            }

                            if (long.TryParse(fileName.Substring(migrationPrefix.Length, fileName.IndexOf(migrationSeparator) - migrationPrefix.Length), out var version) && version > currentVersion && version <= targetVersion)
                            {
                                migrationInfos.Add(new MigrationInfo
                                {
                                    Version = version,
                                    Description = fileName,
                                    ScriptFilePath = scriptFilePath
                                });
                            }
                        }
                    }
                }
                else
                {
                    foreach (var assembly in _migrationOptions.Assemblies)
                    {
                        var resourceNames = assembly.GetManifestResourceNames().Where(c => c.StartsWith(_migrationOptions.EmbeddedScriptNamespace) && c.Length >= _migrationOptions.EmbeddedScriptNamespace.Length + 2).ToList();
                        resourceNames.ForEach(resourceName =>
                        {
                            var fileName = resourceName.Substring(_migrationOptions.EmbeddedScriptNamespace.Length + 1);
                            if (string.IsNullOrEmpty(fileName)
                                || !fileName.StartsWith(_migrationOptions.ScriptOptions.MigrationPrefix)
                                || !fileName.Contains(_migrationOptions.ScriptOptions.MigrationSeparator)
                                || !fileName.EndsWith(migrationScriptExtension))
                            {
                                return;
                            }

                            if (long.TryParse(fileName.Substring(migrationPrefix.Length, fileName.IndexOf(migrationSeparator) - migrationPrefix.Length), out var version) && version > currentVersion && version <= targetVersion)
                            {
                                migrationInfos.Add(new MigrationInfo
                                {
                                    Version = version,
                                    Description = fileName,
                                    ScriptFilePath = resourceName,
                                    EmbeddedScript = true,
                                    Assembly = assembly
                                });
                            }
                        });
                    }
                }
            }
            #endregion

            if (!migrationInfos.Any()) return;

            #region 2. Execute migration.
            foreach (var migrationInfo in migrationInfos.OrderBy(c => c.Version))
            {
                timeRecord.Restart();
                bool success = false;
                ExceptionContext exceptionContext = null;
                string errMsg = null;
                try
                {
                    var sqlList = new List<string>();

                    if (migrationInfo.MigrationClassType != null)
                    {
                        Migration instance = _migrationOptions.MigrationInstanceFactory?.Invoke(migrationInfo.MigrationClassType)
                                             ?? (Migration)Activator.CreateInstance(migrationInfo.MigrationClassType);
                        instance.Upgrade();
                        var migrationItems = instance.MigrationItems;
                        if (migrationItems == null || !migrationItems.Any())
                        {
                            if (!_migrationOptions.AllowEmptyMigrationItems)
                            {
                                success = false;
                                errMsg = "<MigrationItems> is empty.";
                                break;
                            }
                            else
                            {
                                success = true;
                                continue;
                            }
                        }

                        foreach (var migrationItem in migrationItems)
                        {
                            switch (migrationItem.Type)
                            {
                                case MigrationType.Sql:
                                {
                                    sqlList.Add(migrationItem.Data);
                                }
                                    break;
                                case MigrationType.Script:
                                {
                                    if (string.IsNullOrEmpty(_migrationOptions.ScriptBaseDirectory))
                                    {
                                        sqlList.Add(migrationItem.Data);
                                    }
                                    else
                                    {
                                        var sql = File.ReadAllText(Path.Combine(_migrationOptions.ScriptBaseDirectory, migrationItem.Data), Encoding.UTF8);
                                        sqlList.Add(sql);
                                    }
                                }
                                    break;
                                case MigrationType.EmbeddedScript:
                                {
                                    // Embedded resource name: namespace.resourceName
                                    var embeddedScript = $"{_migrationOptions.EmbeddedScriptNamespace}.{migrationItem.Data}";
                                    using (Stream stream = (migrationInfo.Assembly ?? migrationInfo.MigrationClassType.Assembly).GetManifestResourceStream(embeddedScript))
                                    {
                                        if (stream == null)
                                        {
                                            throw new InvalidOperationException($"The embedded resource was not found: {embeddedScript}");
                                        }

                                        using (StreamReader sr = new StreamReader(stream))
                                        {
                                            var sql = sr.ReadToEnd();
                                            sqlList.Add(sql);
                                        }
                                    }
                                }
                                    break;
                                default:
                                    throw new NotImplementedException();
                            }
                        }
                    }
                    else if (!string.IsNullOrEmpty(migrationInfo.ScriptFilePath))
                    {
                        if (!migrationInfo.EmbeddedScript)
                        {
                            var sql = File.ReadAllText(migrationInfo.ScriptFilePath, Encoding.UTF8);
                            sqlList.Add(sql);
                        }
                        else
                        {
                            using (Stream stream = migrationInfo.Assembly.GetManifestResourceStream(migrationInfo.ScriptFilePath))
                            {
                                if (stream == null)
                                {
                                    throw new InvalidOperationException($"The embedded resource was not found: {migrationInfo.ScriptFilePath}");
                                }

                                using (StreamReader sr = new StreamReader(stream))
                                {
                                    var sql = sr.ReadToEnd();
                                    sqlList.Add(sql);
                                }
                            }
                        }
                    }

                    if (!sqlList.Any())
                    {
                        success = false;
                        errMsg = "<MigrationSQL> is empty.";
                        break;
                    }

                    success = _migrationFactory.ExecuteSql(sqlList);

                    if (!success)
                    {
                        break;
                    }
                }
                catch (Exception ex)
                {
                    success = false;
                    exceptionContext = new ExceptionContext(ex) { AddHistory = true };
                    _migrationFactory.OnException(exceptionContext);
                    break;
                }
                finally
                {
                    timeRecord.Stop();
                    if (exceptionContext == null || exceptionContext.AddHistory)
                    {
                        var elapsedMilliseconds = timeRecord.ElapsedMilliseconds;
                        _migrationFactory.Add(entity =>
                        {
                            entity.Version = migrationInfo.Version;
                            entity.MigrationClass = migrationInfo.MigrationClassType?.FullName;
                            entity.Success = success;
                            entity.Description = success ? migrationInfo.Description : exceptionContext?.Exception?.ToString() ?? errMsg;
                            entity.ExecutionTime = DateTime.Now;
                            entity.ExecutionElapsed = elapsedMilliseconds;
                        });
                    }
                }
            }
            #endregion
        }
    }

    public void Rollback(long targetVersion)
    {
        throw new NotImplementedException();
    }

    public void Execute(long targetVersion)
    {
        throw new NotImplementedException();
    }
}