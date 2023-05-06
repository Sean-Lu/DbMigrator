using Example.Domain.Entities;

namespace Example.Application.Contracts
{
    public interface IMigrationHistoryService
    {
        bool Add(MigrationHistoryEntity model);
    }
}