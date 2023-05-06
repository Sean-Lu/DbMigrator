using System.Collections.Generic;

namespace Sean.Core.DbMigrator;

public abstract class Migration : IMigration
{
    public virtual ICollection<MigrationItem> MigrationItems { get; } = new List<MigrationItem>();
    /// <summary>
    /// Gets a value indicating whether it can be rolled back. The default value is true.
    /// </summary>
    public virtual bool CanRollback => true;

    public abstract void Upgrade();

    public abstract void Rollback();

    protected virtual void ExecuteSql(string sql)
    {
        MigrationItems.Add(new MigrationItem
        {
            Type = MigrationType.Sql,
            Data = sql
        });
    }

    protected virtual void ExecuteScript(string sqlScriptName)
    {
        MigrationItems.Add(new MigrationItem
        {
            Type = MigrationType.Script,
            Data = sqlScriptName
        });
    }

    protected virtual void ExecuteEmbeddedScript(string embeddedSqlScriptName)
    {
        MigrationItems.Add(new MigrationItem
        {
            Type = MigrationType.EmbeddedScript,
            Data = embeddedSqlScriptName
        });
    }
}