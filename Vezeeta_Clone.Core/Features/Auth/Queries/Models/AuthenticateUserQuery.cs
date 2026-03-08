using MediatR;
using Vezeeta_Clone.Core.Bases;

namespace Vezeeta_Clone.Core.Features.Auth.Queries.Models
{
    public class AuthenticateUserQuery : IRequest<Response<string>>
    {
        public string AccessToken { get; set; }
    }
}
