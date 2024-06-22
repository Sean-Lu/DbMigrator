using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Example.Domain.Contracts;
using Sean.Core.DbMigrator;

namespace Example.Domain.Entities
{
    [Table("__MigrationHistory")]
    public class MigrationHistoryEntity : IMigrationHistoryEntity, IEntityId
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual long Id { get; set; }
        public virtual long Version { get; set; }
        public virtual string MigrationClass { get; set; }
        public virtual bool Success { get; set; }
        public virtual string Description { get; set; }
        public virtual DateTime ExecutionTime { get; set; }
        public virtual long ExecutionElapsed { get; set; }
    }
}