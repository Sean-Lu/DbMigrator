using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using Example.Domain.Contracts;
using Example.Domain.Entities;
using Sean.Core.DbMigrator;
using Sean.Core.DbRepository;
using Sean.Utility.Contracts;

namespace Example.Domain.DB
{
    public class MigrationFactory : IMigrationFactory
    {
        private readonly IMigrationHistoryRepository _migrationHistoryRepository;
        private readonly ILogger _logger;

        public MigrationFactory(
            IMigrationHistoryRepository migrationHistoryRepository,
            ILogger<MigrationFactory> logger
            )
        {
            _migrationHistoryRepository = migrationHistoryRepository;
            _logger = logger;
        }

        //public void CreateTableIfNotExist()
        //{
        //    // Check whether the table exists.
        //    if (_migrationHistoryRepository.IsTableExists(_migrationHistoryRepository.TableName()))
        //    {
        //        // Table already exists.
        //        return;
        //    }

        //    // Create 'MigrationHistory' table.
        //    var sql = _migrationHistoryRepository.CreateTableSql(_migrationHistoryRepository.TableName());
        //    _migrationHistoryRepository.Execute(c => c.Execute(sql));
        //}

        public long GetCurrentVersion()
        {
            return _migrationHistoryRepository.GetCurrentVersion();
        }

        public bool Add(Action<IMigrationHistoryEntity> action)
        {
            var entity = new MigrationHistoryEntity();
            action(entity);
            return _migrationHistoryRepository.Add(entity);
        }

        public bool IsExecuted(long version)
        {
            var migrationHistoryEntity = _migrationHistoryRepository.Get(entity => entity.Success && entity.Version == version);
            return migrationHistoryEntity != null && migrationHistoryEntity.Id > 0;
        }

        public bool ExecuteSql(IList<string> sqlList)
        {
            if (sqlList == null || !sqlList.Any())
            {
                return true;
            }

            return _migrationHistoryRepository.Execute(conn =>
            {
                foreach (var sql in sqlList)
                {
                    if (string.IsNullOrEmpty(sql)) continue;

                    conn.Execute(sql);
                }

                return true;
            });
        }

        public void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            if (exception == null)
            {
                return;
            }

            _logger.LogError("Database migration error.", exception);// Log exception information.

            //context.AddHistory = false;// Whether to insert migration history. The default value is true.
        }
    }
}
