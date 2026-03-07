using MediatR;
using Vezeeta_Clone.Core.Bases;

namespace Vezeeta_Clone.Core.Features.Auth.Commands.Models
{
    public class UpdateRoleCommand : IRequest<Response<string>>
    {
        public string Id { get; set; }
        public string RoleName { get; set; }
    }
}
