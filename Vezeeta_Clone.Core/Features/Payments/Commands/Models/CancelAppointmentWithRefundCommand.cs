using MediatR;
using System.Text.Json.Serialization;
using Vezeeta_Clone.Core.Bases;

namespace Vezeeta_Clone.Core.Features.Payments.Commands.Models
{
    public class CancelAppointmentWithRefundCommand : IRequest<Response<string>>
    {
        [JsonIgnore]
        public int AppointmentId { get; set; }
        public string? CancellationReason { get; set; }
    }
}
