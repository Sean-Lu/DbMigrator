using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Example.Domain.Entities
{
    [Table("Test001")]
    [Description("测试表")]
    public class TestEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Description("主键")]
        public virtual long Id { get; set; }
        [Description("名称")]
        public virtual string Name { get; set; }
    }
}
