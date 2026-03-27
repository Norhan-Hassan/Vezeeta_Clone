using MediatR;
using Vezeeta_Clone.Core.Bases;

namespace Vezeeta_Clone.Core.Features.Payments.Commands.Models
{
    public class ConfirmPaymentCommand : IRequest<Response<string>>
    {
        public int PaymentId { get; set; }
        public string PaymentMethodId { get; set; }
    }
}