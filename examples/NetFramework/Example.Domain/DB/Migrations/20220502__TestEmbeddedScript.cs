using Sean.Core.DbMigrator;
using System;

namespace Example.Domain.DB.Migrations
{
    [Migration(20220502113010, Description = "测试执行嵌入的SQL脚本")]//yyyyMMddHHmmss
    public class _20220502__TestEmbeddedScript : Migration
    {
        public override bool CanRollback => false;

        public override void Upgrade()
        {
            ExecuteEmbeddedScript("20220502__TestEmbeddedScript.sql");
        }

        public override void Rollback()
        {
            throw new NotImplementedException();
        }
    }
}
