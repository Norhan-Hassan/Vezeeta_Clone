using MediatR;
using Vezeeta_Clone.Core.Bases;

namespace Vezeeta_Clone.Core.Features.Payments.Commands.Models
{
    public class UpdateAppointmentAfterPaymentCommand : IRequest<Response<string>>
    {
        public int PaymentId { get; set; }
        public bool IsPaid { get; set; }
    }
}
