using MediatR;
using Vezeeta_Clone.Core.Bases;
using Vezeeta_Clone.Core.Features.Auth.Shared;
using Vezeeta_Clone.Data.Entities.Enums;

namespace Vezeeta_Clone.Core.Features.Auth.Commands.Models
{
    public class RegisterDoctorCommand : RegisterUserBase, IRequest<Response<string>>
    {
        public Title Title { get; set; }
        public string Description { get; set; }
        public int ExperienceInYears { get; set; }
        public int UniversityId { get; set; }
    }
}
