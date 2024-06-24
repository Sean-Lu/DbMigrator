using Sean.Core.DbMigrator;

namespace Example.Domain.DB.Migrations
{
    [Migration(20220501103010, Description = "初始化数据库")]
    public class _20220501__InitDatabase : Migration
    {
        public override bool CanRollback => true;

        public override void Upgrade()
        {
            ExecuteScript("20220501__InitDatabase.sql");


            //ExecuteSql(@"CREATE TABLE `Test001` (
            //  `Id` INTEGER NOT NULL,
            //  `Name` TEXT,
            //  PRIMARY KEY (`Id`)
            //);");
            //ExecuteSql(@"CREATE TABLE `Test002` (
            //  `Id` INTEGER NOT NULL,
            //  `Name` TEXT,
            //  PRIMARY KEY (`Id`)
            //);");
        }

        public override void Rollback()
        {
            ExecuteSql(@"DROP TABLE Test001;");
            ExecuteSql(@"DROP TABLE Test002;");
        }
    }
}
