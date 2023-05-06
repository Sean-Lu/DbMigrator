using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
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
                    options.ScriptBaseDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Join("\\", sqlScriptPaths));// Base directory for SQL scripts.
                    options.EmbeddedScriptNamespace = $"{sqlAssemblyName}.{string.Join(".", sqlScriptPaths)}";// Namespace for embedded SQL scripts.
                    options.AllowEmptyMigrationItems = true;// Whether to allow empty <MigrationItems> in migration classes.
                    //options.MigrationFilter = type => type.Namespace == $"{sqlAssemblyName}.{string.Join(".", migrationPaths)}";// Migration class filter.
                    options.MigrationInstanceFactory = migrationClassType =>
                    {
                        if (typeof(Migration).IsAssignableFrom(migrationClassType))
                        {
                            // Using dependency Injection for migration classes.
                            return (Migration)DependencyManager.Container.Resolve(migrationClassType);
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
            migrationRunner.Upgrade();
            //migrationRunner.Execute(20220502113510);// Only the specified version is executed, ignoring the current database version.
        }
    }
}
