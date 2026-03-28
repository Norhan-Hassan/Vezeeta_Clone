using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Vezeeta_Clone.Api.Base;
using Vezeeta_Clone.Core.Features.Payments.Commands.Models;
using Vezeeta_Clone.Core.Features.Payments.Queries.Models;
using Vezeeta_Clone.Data.AppMetaData;
using Vezeeta_Clone.Data.Commons;

namespace Vezeeta_Clone.Api.Controllers.V1
{
    [ApiVersion("1.0")]
    [Authorize(Roles = Roles.Patient)]
    public class PaymentsController : AppControllerBase
    {
        [HttpPost(Router.PaymentRouting.CreatePaymentIntent)]
        [SwaggerOperation(Summary = "Create payment intent", Description = "Initiate payment for appointment booking. Supports Stripe card payments and cash payment options")]
        public async Task<IActionResult> CreatePaymentIntent([FromBody] CreatePaymentIntentCommand request)
        {
            var response = await _mediator.Send(request);
            return NewResult(response);
        }

        [HttpPost(Router.PaymentRouting.ConfirmPayment)]
        [SwaggerOperation(Summary = "Confirm Stripe payment", Description = "Confirm payment after Stripe payment method submission and validate transaction")]
        public async Task<IActionResult> ConfirmPayment([FromBody] ConfirmPaymentCommand request)
        {
            var response = await _mediator.Send(request);
            return NewResult(response);
        }

        [HttpPost(Router.PaymentRouting.UpdateAppointmentAfterPayment)]
        [SwaggerOperation(Summary = "Update appointment after payment", Description = "Finalize appointment confirmation after successful payment and send confirmation email")]
        public async Task<IActionResult> UpdateAppointmentAfterPayment([FromBody] UpdateAppointmentAfterPaymentCommand request)
        {
            var response = await _mediator.Send(request);
            return NewResult(response);
        }

        [HttpGet(Router.PaymentRouting.GetPaymentByAppointmentId)]
        [SwaggerOperation(Summary = "Get payment details by appointment", Description = "Retrieve payment information and status for a specific appointment")]
        public async Task<IActionResult> GetPaymentByAppointmentId([FromRoute] int Id)
        {
            var response = await _mediator.Send(new GetPaymentByAppointmentIdQuery { AppointmentId = Id });
            return NewResult(response);
        }


        [HttpPost(Router.PaymentRouting.CancellWithRefund)]
        [SwaggerOperation(Summary = "Cancel appointment with refund", Description = "Cancel confirmed appointment and process refund for Stripe and cash payments")]
        public async Task<IActionResult> CancellWithRefund([FromRoute] int Id, CancelAppointmentWithRefundCommand command)
        {
            command.AppointmentId = Id;
            var response = await _mediator.Send(command);
            return NewResult(response);
        }
    }
}
