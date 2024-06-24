using Example.Application.Dtos;
using System.Collections.Generic;

namespace Example.Application.Contracts
{
    public interface IMigrationHistoryService
    {
        List<MigrationHistoryDto> Search(int? pageNumber = null, int? pageSize = null);
    }
}