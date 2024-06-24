using Example.Application.Contracts;
using Example.Domain.Contracts;
using Example.Domain.Entities;
using Sean.Core.DbRepository;
using System.Collections.Generic;
using System.Linq;
using Example.Application.Dtos;
using Sean.Utility.Format;

namespace Example.Application.Services
{
    public class MigrationHistoryService : IMigrationHistoryService
    {
        private readonly IMigrationHistoryRepository _migrationHistoryRepository;

        public MigrationHistoryService(IMigrationHistoryRepository migrationHistoryRepository)
        {
            _migrationHistoryRepository = migrationHistoryRepository;
        }

        public List<MigrationHistoryDto> Search(int? pageNumber = null, int? pageSize = null)
        {
            var orderBy = OrderByConditionBuilder<MigrationHistoryEntity>.Build(OrderByType.Desc, entity => entity.ExecutionTime);
            orderBy.Next = OrderByConditionBuilder<MigrationHistoryEntity>.Build(OrderByType.Desc, entity => entity.Id);
            return _migrationHistoryRepository.Query(entity => true, orderBy, pageNumber, pageSize)?.Select(entity => ObjectConvert.MapProperties<MigrationHistoryDto>(entity)).ToList();
        }
    }
}
