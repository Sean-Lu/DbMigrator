using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Example.Domain.Entities
{
    [Table("Test001")]
    public class TestEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public virtual long Id { get; set; }
        public virtual string Name { get; set; }
    }
}
