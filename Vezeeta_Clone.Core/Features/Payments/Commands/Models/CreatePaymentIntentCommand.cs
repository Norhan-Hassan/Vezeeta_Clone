using MediatR;
using Vezeeta_Clone.Core.Bases;
using Vezeeta_Clone.Core.Features.Payments.Commands.Results;
using Vezeeta_Clone.Data.Entities.Enums;

namespace Vezeeta_Clone.Core.Features.Payments.Commands.Models
{
    public class CreatePaymentIntentCommand : IRequest<Response<PaymentIntentResponseResult>>
    {
        public int AppointmentId { get; set; }
        public PaymentProvider Provider { get; set; }
    }
}
