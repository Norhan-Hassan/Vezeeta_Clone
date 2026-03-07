using MediatR;
using Vezeeta_Clone.Core.Bases;

namespace Vezeeta_Clone.Core.Features.Auth.Commands.Models
{
    public class AddRoleCommand : IRequest<Response<string>>
    {
        public string RoleName { get; set; }
    }
}
