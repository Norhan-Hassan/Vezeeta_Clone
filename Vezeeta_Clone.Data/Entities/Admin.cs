using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vezeeta_Clone.Data.Entities
{
    public class Admin
    {
        [Key]
        public string Id { get; set; }

        [ForeignKey("ApplicationUser")]
        public string AppUserID { get; set; }
        public ApplicationUser ApplicationUser { get; set; } = default!;
    }
}
