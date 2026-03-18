using MediatR;
using Vezeeta_Clone.Core.Bases;

namespace Vezeeta_Clone.Core.Features.Appointments.Commands.Models
{
    public class ConfirmAppointmentCommand : IRequest<Response<string>>
    {
        public int AppointmentId { get; set; }
    }
}
