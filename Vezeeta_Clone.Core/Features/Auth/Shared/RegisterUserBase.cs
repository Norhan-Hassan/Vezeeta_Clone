using Vezeeta_Clone.Data.Entities.Enums;

namespace Vezeeta_Clone.Core.Features.Auth.Shared
{
    public class RegisterUserBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public Gender gender { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
