namespace Sean.Core.DbMigrator;

public enum MigrationType
{
    Sql,
    Script,
    EmbeddedScript
}

public enum MigrationStep
{
    MigrationNothing,
    MigrationWaiting,
    MigrationItemExecuting,
    MigrationItemExecuted,
    MigrationComplete
}