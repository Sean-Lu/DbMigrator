using Example.Domain.Entities;
using Sean.Core.DbMigrator;
using Sean.Core.DbRepository;

namespace Example.Domain.Contracts
{
    public interface IMigrationHistoryRepository : IBaseRepository<MigrationHistoryEntity>
    {
        /// <summary>
        /// Get current database version.
        /// </summary>
        /// <returns></returns>
        long GetCurrentVersion();
    }
}