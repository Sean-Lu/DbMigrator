using System;

namespace Sean.Core.DbMigrator;

public interface IMigrationBuilder
{
    IMigrationBuilder ConfigureRunner(Action<MigrationOptions> action);

    IMigrationRunner Build(IMigrationFactory migrationFactory);
}