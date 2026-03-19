using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vezeeta_Clone.Api.Base;
using Vezeeta_Clone.Core.Features.Appointments.Commands.Models;
using Vezeeta_Clone.Data.AppMetaData;
using Vezeeta_Clone.Data.Commons;

namespace Vezeeta_Clone.Api.Controllers.V1
{
    [ApiVersion("1")]
    public class AppointmentsController : AppControllerBase
    {
        [Authorize(Roles = Roles.Patient)]
        [HttpPost(Router.AppointmentRouting.BookAppointment)]
        public async Task<IActionResult> BookAppointment([FromBody] BookAppointmentCommand command)
        {
            var response = await _mediator.Send(command);
            return NewResult(response);
        }


        [Authorize(Roles = Roles.Patient)]
        [HttpPost(Router.AppointmentRouting.CompleteAppointmentBooking)]
        public async Task<IActionResult> CompleteAppointmentBooking([FromBody] CompleteAppointmentCommand command)
        {
            var response = await _mediator.Send(command);
            return NewResult(response);
        }


    }
}
