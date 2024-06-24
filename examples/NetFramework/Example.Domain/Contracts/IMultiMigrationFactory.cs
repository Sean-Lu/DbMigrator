using Sean.Core.DbMigrator;
using Sean.Core.DbRepository;

namespace Example.Domain.Contracts
{
    /// <summary>
    /// 多数据库迁移工厂
    /// </summary>
    public interface IMultiMigrationFactory : IMigrationFactory
    {
        /// <summary>
        /// 切换数据库
        /// </summary>
        /// <param name="options"></param>
        void ChangeDatabase(ConnectionStringOptions options);
    }
}
