using System.Linq;
using Example.Domain.Contracts;
using Example.Domain.Entities;
using Sean.Core.DbRepository;
using Sean.Core.DbRepository.Dapper;

namespace Example.Domain.Repositories
{
    public class MigrationHistoryRepository : DapperBaseRepository<MigrationHistoryEntity>, IMigrationHistoryRepository
    {
        public MigrationHistoryRepository() : base(new MultiConnectionSettings(DbContext.MainDbOptions))
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
            var orderBy = OrderByConditionBuilder<MigrationHistoryEntity>.Build(OrderByType.Desc, entity => entity.ExecutionTime);
            orderBy.Next = OrderByConditionBuilder<MigrationHistoryEntity>.Build(OrderByType.Desc, entity => entity.Id);
            var migrationHistoryEntity = Query(entity => entity.Success, orderBy, 1, 1)?.FirstOrDefault();
            return migrationHistoryEntity?.Version ?? -1;
        }

        public void ChangeDatabase(ConnectionStringOptions options)
        {
            Factory.ConnectionSettings.Clear();
            Factory.ConnectionSettings.Add(options);
        }
    }
}
