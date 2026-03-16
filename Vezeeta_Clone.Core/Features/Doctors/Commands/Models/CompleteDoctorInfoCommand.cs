using MediatR;
using Vezeeta_Clone.Core.Bases;

namespace Vezeeta_Clone.Core.Features.Doctors.Commands.Models
{
    public class CompleteDoctorInfoCommand : IRequest<Response<string>>
    {
        public int[]? SubSpecializations { get; set; }
        public string Description { get; set; }
    }
}
