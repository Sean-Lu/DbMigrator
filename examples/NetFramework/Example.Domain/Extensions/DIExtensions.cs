using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Reflection;
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
        public static void AddDomainDI(this IDIRegister container)
        {
            container.AddInfrastructureDI();

            container.RegisterAssemblyByInterfaceSuffix(Assembly.GetExecutingAssembly(), "Repository", ServiceLifeStyle.Transient);
            container.RegisterType<IMigrationFactory, MigrationFactory>(ServiceLifeStyle.Singleton);

            #region Dependency Injection for migration classes.
            var types = Assembly.GetExecutingAssembly().GetTypes().Where(c => c.IsClass && !c.IsAbstract && typeof(IMigration).IsAssignableFrom(c)).ToList();
            types.ForEach(migrationClassType =>
            {
                container.RegisterType(migrationClassType, migrationClassType, ServiceLifeStyle.Transient);
            });
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
                options.BulkEntityCount = 200;
                //options.JsonSerializer = NewJsonSerializer.Instance;
                //options.SqlExecuting += OnSqlExecuting;
                //options.SqlExecuted += OnSqlExecuted;
            });

            #endregion
        }
    }
}
