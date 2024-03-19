using System.Linq;
using Example.Domain.Contracts;
using Example.Domain.Entities;
using Sean.Core.DbMigrator;
using Sean.Core.DbRepository;
using Sean.Core.DbRepository.Dapper;

namespace Example.Domain.Repositories
{
    public class MigrationHistoryRepository : DapperBaseRepository<MigrationHistoryEntity>, IMigrationHistoryRepository
    {
        public MigrationHistoryRepository() : base(DbContext.ConnString, DatabaseType.SQLite)
        {
        }

        public override string TableName()
        {
            var tableName = base.TableName();
            AutoCreateTable(tableName);
            return tableName;
        }

        public long GetCurrentVersion()
        {
            var orderBy = OrderByConditionBuilder<IMigrationHistoryEntity>.Build(OrderByType.Desc, entity => entity.ExecutionTime);
            orderBy.Next = OrderByConditionBuilder<IMigrationHistoryEntity>.Build(OrderByType.Desc, entity => entity.Id);
            var migrationHistoryEntity = Query(entity => entity.Success, orderBy, 1, 1)?.FirstOrDefault();
            return migrationHistoryEntity?.Version ?? -1;
        }
    }
}
