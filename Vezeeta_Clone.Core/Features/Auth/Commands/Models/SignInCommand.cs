using MediatR;
using Vezeeta_Clone.Core.Bases;
using Vezeeta_Clone.Data.Results;

namespace Vezeeta_Clone.Core.Features.Auth.Commands.Models
{
    public class SignInCommand : IRequest<Response<JwtAuthResult>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
