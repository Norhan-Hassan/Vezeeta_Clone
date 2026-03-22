using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Vezeeta_Clone.Api.Base;
using Vezeeta_Clone.Core.Features.Patients.Queries.Models;
using Vezeeta_Clone.Data.AppMetaData;

namespace Vezeeta_Clone.Api.Controllers.V1
{
    [ApiVersion("1")]
    public class PatientsController : AppControllerBase
    {
        [HttpGet(Router.PatientRouting.AppointmentsList)]
        [SwaggerOperation(Summary = "Appoinments of Patient ", Description = "Get paginated list of appointments of the current patient")]

        public async Task<IActionResult> GetDoctorsPaginated([FromQuery] GetPatientAppointmentsQuery query)
        {
            var response = await _mediator.Send(query);
            return NewResult(response);
        }
    }
}
