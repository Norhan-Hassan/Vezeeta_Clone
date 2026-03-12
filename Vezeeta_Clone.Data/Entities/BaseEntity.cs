using System.ComponentModel.DataAnnotations;
using Vezeeta_Clone.Data.Commons;

namespace Vezeeta_Clone.Data.Entities
{
    public class BaseEntity : LocalizableEntity
    {
        [Key]
        public int ID { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
