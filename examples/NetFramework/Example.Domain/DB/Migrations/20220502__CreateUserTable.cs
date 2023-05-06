using Sean.Core.DbMigrator;
using System;

namespace Example.Domain.DB.Migrations
{
    [Migration(20220502103010, Description = "新增用户表")]//yyyyMMddHHmmss
    public class _20220502__CreateUserTable : Migration
    {
        public override bool CanRollback => false;

        public override void Upgrade()
        {
            ExecuteScript("20220502__CreateUserTable.sql");
        }

        public override void Rollback()
        {
            throw new NotImplementedException();
        }
    }
}
