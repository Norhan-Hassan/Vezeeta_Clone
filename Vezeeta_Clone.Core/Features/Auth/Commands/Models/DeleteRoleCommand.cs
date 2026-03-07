using MediatR;
using Vezeeta_Clone.Core.Bases;

namespace Vezeeta_Clone.Core.Features.Auth.Commands.Models
{
    public class DeleteRoleCommand : IRequest<Response<string>>
    {
        public string Id { get; set; }
    }
}
