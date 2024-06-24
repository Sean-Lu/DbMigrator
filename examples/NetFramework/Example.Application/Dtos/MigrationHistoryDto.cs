using System;
using Example.Domain.Contracts;
using Sean.Core.DbMigrator;

namespace Example.Application.Dtos
{
    public class MigrationHistoryDto : IMigrationHistoryEntity, IEntityId
    {
        public virtual long Id { get; set; }
        public virtual long Version { get; set; }
        public virtual string MigrationClass { get; set; }
        public virtual bool Success { get; set; }
        public virtual string Description { get; set; }
        public virtual DateTime ExecutionTime { get; set; }
        public virtual long ExecutionElapsed { get; set; }
    }
}
