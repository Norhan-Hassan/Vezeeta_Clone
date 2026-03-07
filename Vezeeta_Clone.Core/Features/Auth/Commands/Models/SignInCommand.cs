using MediatR;
using Vezeeta_Clone.Core.Bases;

namespace Vezeeta_Clone.Core.Features.Auth.Commands.Models
{
    public class SignInCommand : IRequest<Response<string>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
