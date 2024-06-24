using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Example.Domain.Contracts;
using Sean.Core.DbMigrator;

namespace Example.Domain.Entities
{
    [Table("__MigrationHistory")]
    [Description("数据库升级历史表")]
    public class MigrationHistoryEntity : IMigrationHistoryEntity, IEntityId
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Description("主键")]
        public virtual long Id { get; set; }
        [Description("版本号")]
        public virtual long Version { get; set; }
        [Description("迁移类")]
        public virtual string MigrationClass { get; set; }
        [Description("是否执行成功")]
        public virtual bool Success { get; set; }
        [Description("描述")]
        public virtual string Description { get; set; }
        [Description("执行时间")]
        public virtual DateTime ExecutionTime { get; set; }
        [Description("执行耗时")]
        public virtual long ExecutionElapsed { get; set; }
    }
}