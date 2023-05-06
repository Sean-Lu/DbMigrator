using System;
using System.Reflection;

namespace Sean.Core.DbMigrator;

internal class MigrationInfo
{
    /// <summary>
    /// Migration version
    /// </summary>
    public long Version { get; set; }
    /// <summary>
    /// Migration description
    /// </summary>
    public string Description { get; set; }
    /// <summary>
    /// Migration class type
    /// </summary>
    public Type MigrationClassType { get; set; }
    public string ScriptFilePath { get; set; }
    public bool EmbeddedScript { get; set; }
    public Assembly Assembly { get; set; }
}