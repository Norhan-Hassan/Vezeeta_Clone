using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> CreateSpecialization([FromBody] RegisterClinicForDoctorCommand request)
        {
            var response = await _mediator.Send(request);
            return NewResult(response);
        }


    }
}
