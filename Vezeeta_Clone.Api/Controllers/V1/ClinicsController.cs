using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Vezeeta_Clone.Api.Base;
using Vezeeta_Clone.Core.Features.Clinics.Commands.Models;
using Vezeeta_Clone.Data.AppMetaData;
using Vezeeta_Clone.Data.Commons;

namespace Vezeeta_Clone.Api.Controllers.V1
{
    [ApiVersion("1")]
    [Authorize(Roles = Roles.Doctor)]
    public class ClinicsController : AppControllerBase
    {
        [HttpPost(Router.ClinicRouting.RegisterClinic)]
        [SwaggerOperation(Summary = "Register clinic for doctor", Description = "Register a new clinic with address, location, phone number and consultation price , doctor complete profile is required before")]
        public async Task<IActionResult> CreateSpecialization([FromBody] RegisterClinicForDoctorCommand request)
        {
            var response = await _mediator.Send(request);
            return NewResult(response);
        }
    }
}
