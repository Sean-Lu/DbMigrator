namespace Sean.Core.DbMigrator;

public interface IMigration
{
    void Upgrade();
    void Rollback();
}