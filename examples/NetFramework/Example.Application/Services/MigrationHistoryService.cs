using Example.Application.Contracts;
using Example.Domain.Contracts;
using Example.Domain.Entities;
using Example.Domain.Repositories;

namespace Example.Application.Services
{
    public class MigrationHistoryService : IMigrationHistoryService
    {
        private readonly IMigrationHistoryRepository _migrationHistoryRepository;

        public MigrationHistoryService(IMigrationHistoryRepository migrationHistoryRepository)
        {
            _migrationHistoryRepository = migrationHistoryRepository;
        }

        public bool Add(MigrationHistoryEntity model)
        {
            return _migrationHistoryRepository.Add(model);
        }
    }
}
