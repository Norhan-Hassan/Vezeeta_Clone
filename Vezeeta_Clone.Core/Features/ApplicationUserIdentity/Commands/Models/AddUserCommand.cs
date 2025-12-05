using MediatR;
using Vezeeta_Clone.Core.Bases;
using Vezeeta_Clone.Data.Entities.Enums;

namespace Vezeeta_Clone.Core.Features.ApplicationUserIdentity.Commands.Models
{
    public class AddUserCommand : IRequest<Response<string>>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public Gender Gender { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
