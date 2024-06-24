using Sean.Core.DbRepository;

namespace Example.Domain
{
    public class DbContext
    {
        /// <summary>
        /// 主数据库
        /// </summary>
        public static readonly ConnectionStringOptions MainDbOptions = ConnectionStringOptions.Create(@"data source=.\test.db;pooling=True;journal mode=Wal", DatabaseType.SQLite);

        public static DatabaseType DatabaseType => MainDbOptions.DbType;
    }
}
