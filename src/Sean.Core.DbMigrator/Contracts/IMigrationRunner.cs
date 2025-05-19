using System;

namespace Sean.Core.DbMigrator;

public interface IMigrationRunner
{
    bool Upgrade(Action<MigrationCallbackContext> callback = null);
    bool Upgrade(long targetVersion, Action<MigrationCallbackContext> callback = null);
    bool Rollback(long targetVersion);
    bool Execute(long targetVersion);
}