using Sean.Core.DbMigrator;
using System;
using Example.Domain.Contracts;
using Example.Domain.Entities;
using Example.Infrastructure;
using System.Diagnostics;

namespace Example.Domain.DB.Migrations
{
    [Migration(20220503113010, Description = "测试执行自定义升级和回滚操作")]//yyyyMMddHHmmss
    public class _20220503__TestEmptyMigrationItems : Migration
    {
        public override bool CanRollback => true;

        private readonly ITestRepository _testRepository;

        //public _20220503__TestEmptyMigrationItems()
        //{
        //    // Using dependency Injection in migration class.
        //    _testRepository = DependencyManager.Container.Resolve<ITestRepository>();
        //}

        // Using dependency Injection for migration classes.
        public _20220503__TestEmptyMigrationItems(
            ITestRepository testRepository
            )
        {
            _testRepository = testRepository;
        }

        public override void Upgrade()
        {
            _testRepository.Add(new TestEntity
            {
                Id = 1,
                Name = "Test001"
            });
            _testRepository.Add(new TestEntity
            {
                Id = 2,
                Name = "Test002"
            });
        }

        public override void Rollback()
        {
            _testRepository.Delete(entity => entity.Id == 1);
            _testRepository.Delete(entity => entity.Id == 2);
        }
    }
}
