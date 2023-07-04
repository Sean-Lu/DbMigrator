using Example.Domain.Contracts;
using Example.Domain.Entities;
using Sean.Core.DbRepository;
using Sean.Core.DbRepository.Dapper;

namespace Example.Domain.Repositories
{
    public class TestRepository : DapperBaseRepository<TestEntity>, ITestRepository
    {
        public TestRepository() : base(DbContext.ConnString, DatabaseType.SQLite)
        {
        }
    }
}
