using System;
using System.Collections.Generic;
using System.Reflection;

namespace Sean.Core.DbMigrator;

public class MigrationOptions
{
    public ICollection<Assembly> Assemblies { get; set; }
    /// <summary>
    /// Optional, base directory for SQL scripts, used for <see cref="Migration.ExecuteScript"/>
    /// </summary>
    public string ScriptBaseDirectory { get; set; }
    /// <summary>
    /// Optional, namespace for embedded SQL scripts, used for <see cref="Migration.ExecuteEmbeddedScript"/>
    /// </summary>
    public string EmbeddedScriptNamespace { get; set; }
    /// <summary>
    /// Optional, whether to allow empty <see cref="Migration.MigrationItems"/> in migration classes.
    /// </summary>
    public bool AllowEmptyMigrationItems { get; set; }
    /// <summary>
    /// Migration class filter.
    /// </summary>
    public Func<Type, bool> MigrationFilter { get; set; }
    /// <summary>
    /// Optional, the factory that gets an instance of the migration class.
    /// </summary>
    public Func<Type, Migration> MigrationInstanceFactory { get; set; }
    /// <summary>
    /// Optional, whether to automatically execute database migration from SQL script files: V{migrationVersion}__{migrationDescription}.sql.
    /// </summary>
    public bool AutoMigrateFromFiles { get; set; }
    public MigrationScriptOptions ScriptOptions { get; set; }
}