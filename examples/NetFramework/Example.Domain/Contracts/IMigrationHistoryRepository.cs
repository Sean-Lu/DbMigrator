using Example.Domain.Entities;
using Sean.Core.DbRepository;

namespace Example.Domain.Contracts
{
    public interface IMigrationHistoryRepository : IBaseRepository<MigrationHistoryEntity>
    {
        /// <summary>
        /// 获取当前版本号
        /// </summary>
        /// <returns></returns>
        long GetCurrentVersion();
        /// <summary>
        /// 切换数据库
        /// </summary>
        /// <param name="options"></param>
        void ChangeDatabase(ConnectionStringOptions options);
    }
}