using MediatR;
using Vezeeta_Clone.Core.Bases;

namespace Vezeeta_Clone.Core.Features.Specializations.Commands.Models
{
    public class CreateSpecializationCommand : IRequest<Response<string>>
    {
        public string Name { get; set; }
        public string? Description { get; set; }

    }
}
