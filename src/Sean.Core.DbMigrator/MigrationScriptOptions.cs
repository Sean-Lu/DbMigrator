namespace Sean.Core.DbMigrator;

public class MigrationScriptOptions
{
    /// <summary>
    /// Migration SQL script file name prefix, default value is "V".
    /// </summary>
    public string MigrationPrefix { get; set; } = "V";
    /// <summary>
    /// Migration SQL script file name separator (between version number and description), default value is "__".
    /// </summary>
    public string MigrationSeparator { get; set; } = "__";
    /// <summary>
    /// Migration SQL script file name extension, default value is ".sql".
    /// </summary>
    public string MigrationScriptExtension { get; set; } = ".sql";
    /// <summary>
    /// Whether the SQL script file is an embedded resource.
    /// </summary>
    public bool EmbeddedScript { get; set; }
}