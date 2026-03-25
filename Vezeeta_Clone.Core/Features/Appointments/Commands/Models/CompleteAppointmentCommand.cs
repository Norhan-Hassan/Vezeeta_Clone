using MediatR;
using System.Text.Json.Serialization;
using Vezeeta_Clone.Core.Bases;

namespace Vezeeta_Clone.Core.Features.Appointments.Commands.Models
{
    public class CompleteAppointmentCommand : IRequest<Response<string>>
    {
        [JsonIgnore]
        public int AppointmentId { get; set; }
        public string? Notes { get; set; }
    }
}
