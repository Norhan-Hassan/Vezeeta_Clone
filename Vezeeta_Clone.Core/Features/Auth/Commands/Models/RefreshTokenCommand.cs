using MediatR;
using Vezeeta_Clone.Core.Bases;
using Vezeeta_Clone.Data.Results;

namespace Vezeeta_Clone.Core.Features.Auth.Commands.Models
{
    public class RefreshTokenCommand : IRequest<Response<JwtAuthResult>>
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
