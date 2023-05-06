namespace Sean.Core.DbMigrator;

public interface IMigrationRunner
{
    void Upgrade();
    void Upgrade(long targetVersion);
    void Rollback(long targetVersion);
    void Execute(long targetVersion);
}