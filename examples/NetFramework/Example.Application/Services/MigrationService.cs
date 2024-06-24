using System;
using System.IO;
using Example.Application.Contracts;
using Example.Domain.DB.Migrations;
using Example.Infrastructure;
using Sean.Core.DbMigrator;

namespace Example.Application.Services
{
    public class MigrationService : IMigrationService
    {
        private readonly IMigrationFactory _migrationFactory;

        public MigrationService(IMigrationFactory migrationFactory)
        {
            _migrationFactory = migrationFactory;
        }

        public void Upgrade()
        {
            var migrationBuilder = MigrationManager.CreateDefaultBuilder()
                .ConfigureRunner(options =>
                {
                    var sqlAssemblyName = "Example.Domain";
                    var sqlScriptPaths = new[] { "DB", "Scripts" };
                    var migrationPaths = new[] { "DB", "Migrations" };

                    options.Assemblies = new[] { typeof(_20220501__InitDatabase).Assembly };
                    options.ScriptBaseDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Join("\\", sqlScriptPaths));// SQL脚本相对路径
                    options.EmbeddedScriptNamespace = $"{sqlAssemblyName}.{string.Join(".", sqlScriptPaths)}";// SQL脚本命名空间（嵌入的资源）
                    options.AllowEmptyMigrationItems = true;// Whether to allow empty <MigrationItems> in migration classes.
                    //options.MigrationFilter = type => type.Namespace == $"{sqlAssemblyName}.{string.Join(".", migrationPaths)}";// Migration class filter.
                    options.MigrationInstanceFactory = migrationClassType =>
                    {
                        if (typeof(Migration).IsAssignableFrom(migrationClassType))
                        {
                            // Using dependency Injection for migration classes.
                            return DIManager.Resolve<Migration>(migrationClassType);
                        }

                        return null;
                    };
                    options.AutoMigrateFromFiles = true;// V{migrationVersion}__{migrationDescription}.sql
                    options.ScriptOptions = new MigrationScriptOptions
                    {
                        MigrationPrefix = "V",
                        MigrationSeparator = "__",
                        //EmbeddedScript = true
                    };
                });
            var migrationRunner = migrationBuilder.Build(_migrationFactory);
            //migrationRunner.Execute(20220502113510);
            migrationRunner.Upgrade();
        }
    }
}
