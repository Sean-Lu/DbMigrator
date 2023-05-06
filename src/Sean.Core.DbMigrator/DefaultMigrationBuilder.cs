using System;

namespace Sean.Core.DbMigrator;

public class DefaultMigrationBuilder : IMigrationBuilder
{
    private readonly MigrationOptions _options;

    public DefaultMigrationBuilder()
    {
        _options = new MigrationOptions();
    }

    public IMigrationBuilder ConfigureRunner(Action<MigrationOptions> action)
    {
        action?.Invoke(_options);
        return this;
    }

    public IMigrationRunner Build(IMigrationFactory migrationFactory)
    {
        return new DefaultMigrationRunner(migrationFactory, _options);
    }
}