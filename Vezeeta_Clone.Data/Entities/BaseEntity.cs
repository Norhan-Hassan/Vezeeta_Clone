using System.ComponentModel.DataAnnotations;

namespace Vezeeta_Clone.Data.Entities
{
    public class BaseEntity
    {
        [Key]
        public int ID { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
