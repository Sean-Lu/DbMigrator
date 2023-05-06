using System.Linq;
using Example.Domain.Contracts;
using Example.Domain.Entities;
using Sean.Core.DbMigrator;
using Sean.Core.DbRepository;
using Sean.Core.DbRepository.Dapper;

namespace Example.Domain.Repositories
{
    public class MigrationHistoryRepository : BaseRepository<MigrationHistoryEntity>, IMigrationHistoryRepository
    {
        public MigrationHistoryRepository() : base(DbContext.ConnString, DatabaseType.SQLite)
        {
        }

        protected override void OnSqlExecuting(SqlExecutingContext context)
        {
            base.OnSqlExecuting(context);
        }

        protected override void OnSqlExecuted(SqlExecutedContext context)
        {
            base.OnSqlExecuted(context);
        }

        public override string TableName()
        {
            var tableName = base.TableName();
            AutoCreateTable(tableName);// Automatically creates 'MigrationHistory' table if it does not exist.
            return tableName;
        }

        public override string CreateTableSql(string tableName)
        {
            return $@"CREATE TABLE `{tableName}` (
  `Id` integer NOT NULL PRIMARY KEY AUTOINCREMENT,
  `Version` integer NOT NULL,
  `MigrationClass` text,
  `Success` integer NOT NULL,
  `Description` text,
  `ExecutionTime` text NOT NULL,
  `ExecutionElapsed` integer NOT NULL
);";
        }

        public long GetCurrentVersion()
        {
            // If the 'MigrationHistory' table does not exist, you need to create it before using it.
            // Here, 'IMigrationHistoryRepository' will automatically create the 'MigrationHistory' table.

            var orderBy = OrderByConditionBuilder<IMigrationHistoryEntity>.Build(OrderByType.Desc, entity => entity.ExecutionTime);
            orderBy.Next = OrderByConditionBuilder<IMigrationHistoryEntity>.Build(OrderByType.Desc, entity => entity.Id);
            var migrationHistoryEntity = Query(entity => entity.Success, orderBy, 1, 1)?.FirstOrDefault();
            return migrationHistoryEntity?.Version ?? -1;
        }
    }
}
