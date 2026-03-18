using MediatR;
using Vezeeta_Clone.Core.Bases;

namespace Vezeeta_Clone.Core.Features.Appointments.Commands.Models
{
    public class BookAppointmentCommand : IRequest<Response<string>>
    {
        public string DoctorId { get; set; }
        public int SlotId { get; set; }
    }
}
