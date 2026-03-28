using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Vezeeta_Clone.Api.Base;
using Vezeeta_Clone.Core.Features.Patients.Queries.Models;
using Vezeeta_Clone.Data.AppMetaData;
using Vezeeta_Clone.Data.Commons;

namespace Vezeeta_Clone.Api.Controllers.V1
{
    [ApiVersion("1")]
    [Authorize(Roles = Roles.Patient)]
    public class PatientsController : AppControllerBase
    {
        [HttpGet(Router.PatientRouting.AppointmentsList)]
        [SwaggerOperation(Summary = "Get patient appointments", Description = "Get paginated list of appointments for the current patient with filtering options")]
        public async Task<IActionResult> GetPatientAppointments([FromQuery] GetPatientAppointmentsQuery query)
        {
            var response = await _mediator.Send(query);
            return NewResult(response);
        }
    }
}
