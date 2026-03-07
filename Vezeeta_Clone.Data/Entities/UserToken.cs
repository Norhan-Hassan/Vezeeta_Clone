using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vezeeta_Clone.Data.Entities
{
    [Table("UsersTokens")]
    public class UserToken
    {
        [Key]
        public int ID { get; set; }
        public string? JwtId { get; set; }
        public string? RefreshToken { get; set; }
        public string? AccessToken { get; set; }
        public bool IsUsed { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiredAt { get; set; }

        [ForeignKey(nameof(AppUser))]
        public string UserId { get; set; }
        public ApplicationUser? AppUser { get; set; }
    }
}
