using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Vezeeta_Clone.Api.Base;
using Vezeeta_Clone.Core.Features.Appointments.Commands.Models;
using Vezeeta_Clone.Core.Features.Appointments.Queries.Models;
using Vezeeta_Clone.Data.AppMetaData;
using Vezeeta_Clone.Data.Commons;

namespace Vezeeta_Clone.Api.Controllers.V1
{
    [ApiVersion("1")]
    public class AppointmentsController : AppControllerBase
    {
        [Authorize(Roles = Roles.Patient)]
        [HttpPost(Router.AppointmentRouting.BookAppointment)]
        [SwaggerOperation(Summary = "Book appointment", Description = "Create new appointment booking by selecting a doctor and available time slot", OperationId = "BookThenComplete")]
        public async Task<IActionResult> BookAppointment([FromBody] BookAppointmentCommand command)
        {
            var response = await _mediator.Send(command);
            return NewResult(response);
        }

        [Authorize(Roles = Roles.Patient)]
        [HttpPut(Router.AppointmentRouting.CompleteAppointmentBooking)]
        [SwaggerOperation(Summary = "Complete appointment booking", Description = "Complete appointment booking confirmation process as a step before payment", OperationId = "BookThenComplete")]
        public async Task<IActionResult> CompleteAppointmentBooking([FromRoute] int Id, [FromBody] CompleteAppointmentCommand command)
        {
            command.AppointmentId = Id;
            var response = await _mediator.Send(command);
            return NewResult(response);
        }

        [HttpGet(Router.AppointmentRouting.GetAppointmentDetails)]
        [SwaggerOperation(Summary = "Get appointment details", Description = "Retrieve detailed information for a specific appointment")]
        public async Task<IActionResult> GetAppointmentDetails([FromRoute] GetAppointmentDetailesQuery query)
        {
            var response = await _mediator.Send(query);
            return NewResult(response);
        }
    }
}
