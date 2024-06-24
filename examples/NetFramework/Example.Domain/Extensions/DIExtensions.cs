using System.Data.SQLite;
using System.Linq;
using System.Reflection;
using Example.Domain.Contracts;
using Example.Domain.DB;
using Example.Infrastructure.Extensions;
using Sean.Core.DbMigrator;
using Sean.Core.DbRepository;
using Sean.Core.DbRepository.Extensions;
using Sean.Core.DependencyInjection;

namespace Example.Domain.Extensions
{
    public static class DIExtensions
    {
        /// <summary>
        /// 领域层依赖注入
        /// </summary>
        /// <param name="container"></param>
        public static void AddDomainDI(this IDIRegister container)
        {
            container.AddInfrastructureDI();

            container.RegisterAssemblyByInterfaceSuffix(Assembly.GetExecutingAssembly(), "Repository", ServiceLifeStyle.Transient);

            #region Dependency Injection for migration classes.
            var types = Assembly.GetExecutingAssembly().GetTypes().Where(c => c.IsClass && !c.IsAbstract && typeof(IMigration).IsAssignableFrom(c)).ToList();
            types.ForEach(migrationClassType =>
            {
                container.RegisterType(migrationClassType, migrationClassType, ServiceLifeStyle.Transient);
            });

            container.RegisterType<IMigrationFactory, MigrationFactory>(ServiceLifeStyle.Singleton);
            container.RegisterType<IMultiMigrationFactory, MigrationFactory>(ServiceLifeStyle.Singleton);
            #endregion

            #region Database configuration.

            #region 配置数据库和数据库提供者工厂之间的映射关系
            //DatabaseType.MySql.SetDbProviderMap(new DbProviderMap("MySql.Data.MySqlClient", MySqlClientFactory.Instance));// MySql
            //DatabaseType.SqlServer.SetDbProviderMap(new DbProviderMap("System.Data.SqlClient", SqlClientFactory.Instance));// Microsoft SQL Server
            //DatabaseType.Oracle.SetDbProviderMap(new DbProviderMap("Oracle.ManagedDataAccess.Client", OracleClientFactory.Instance));// Oracle
            DatabaseType.SQLite.SetDbProviderMap(new DbProviderMap("System.Data.SQLite", SQLiteFactory.Instance));// SQLite
            //DatabaseType.SQLite.SetDbProviderMap(new DbProviderMap("System.Data.SQLite", "System.Data.SQLite.SQLiteFactory,System.Data.SQLite"));// SQLite
            #endregion

            DbContextConfiguration.Configure(options =>
            {
                if (DbContext.DatabaseType == DatabaseType.SQLite)
                {
                    options.SynchronousWriteOptions.Enable = true;// 启用同步写入模式：解决多线程并发写入导致的锁库问题
                    options.SynchronousWriteOptions.LockTimeout = 5000;// 同步写入锁等待超时时间（单位：毫秒），默认值：5000
                }

                options.BulkEntityCount = 200;
                //options.JsonSerializer = NewJsonSerializer.Instance;
                //options.SqlExecuting += OnSqlExecuting;
                //options.SqlExecuted += OnSqlExecuted;
            });

            #endregion
        }
    }
}
