using System;
using Sean.Core.DbMigrator;

namespace Example.Application.Contracts
{
    public interface IMigrationService
    {
        bool Upgrade(Action<MigrationCallbackContext> callback = null);
    }
}