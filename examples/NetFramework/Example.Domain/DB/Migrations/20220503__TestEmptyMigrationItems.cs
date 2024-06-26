﻿using Sean.Core.DbMigrator;
using Example.Domain.Contracts;
using Example.Domain.Entities;

namespace Example.Domain.DB.Migrations
{
    [Migration(20220503113010, Description = "测试执行自定义升级和回滚操作")]
    public class _20220503__TestEmptyMigrationItems : Migration
    {
        public override bool CanRollback => true;

        private readonly ITestRepository _testRepository;

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
