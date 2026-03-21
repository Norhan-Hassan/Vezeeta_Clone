using MediatR;
using Vezeeta_Clone.Core.Bases;

namespace Vezeeta_Clone.Core.Features.Appointments.Commands.Models
{
    public class BookAppointmentCommand : IRequest<Response<string>>
    {
        public int SlotId { get; set; }
        public string? ActualPatientName { get; set; }
        public string? ActualPatientPhone { get; set; }
    }
}
