using Microsoft.AspNetCore.Identity;
using Vezeeta_Clone.Data.Entities.Enums;

namespace Vezeeta_Clone.Data.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender gender { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true; // [for soft delete rather than the IsDeleted property in BasEntity]
        public ICollection<Notification> Notifications { get; set; }
        public ICollection<UserToken> Tokens { get; set; } = new HashSet<UserToken>();
    }
}
