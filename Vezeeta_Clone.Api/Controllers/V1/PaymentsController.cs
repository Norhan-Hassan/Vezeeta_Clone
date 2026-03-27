using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> CreatePaymentIntent([FromBody] CreatePaymentIntentCommand request)
        {
            var response = await _mediator.Send(request);
            return NewResult(response);
        }

        [HttpPost(Router.PaymentRouting.ConfirmPayment)]
        public async Task<IActionResult> ConfirmPayment([FromBody] ConfirmPaymentCommand request)
        {
            var response = await _mediator.Send(request);
            return NewResult(response);
        }

        [HttpPost(Router.PaymentRouting.UpdateAppointmentAfterPayment)]
        public async Task<IActionResult> UpdateAppointmentAfterPayment([FromBody] UpdateAppointmentAfterPaymentCommand request)
        {
            var response = await _mediator.Send(request);
            return NewResult(response);
        }

        [HttpGet(Router.PaymentRouting.GetPaymentByAppointmentId)]
        public async Task<IActionResult> GetPaymentByAppointmentId([FromRoute] int Id)
        {
            var response = await _mediator.Send(new GetPaymentByAppointmentIdQuery { AppointmentId = Id });
            return NewResult(response);
        }


        [HttpPost(Router.PaymentRouting.CancellWithRefund)]
        public async Task<IActionResult> CancellWithRefund([FromRoute] int Id, CancelAppointmentWithRefundCommand command)
        {
            command.AppointmentId = Id;
            var response = await _mediator.Send(command);
            return NewResult(response);
        }
    }
}
