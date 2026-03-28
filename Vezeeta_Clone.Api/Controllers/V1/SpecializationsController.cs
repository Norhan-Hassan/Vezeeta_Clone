using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Vezeeta_Clone.Api.Base;
using Vezeeta_Clone.Core.Features.Specializations.Commands.Models;
using Vezeeta_Clone.Core.Features.Specializations.Queries.Models;
using Vezeeta_Clone.Data.AppMetaData;
using Vezeeta_Clone.Data.Commons;

namespace Vezeeta_Clone.Api.Controllers.V1
{
    [ApiVersion("1")]
    public class SpecializationsController : AppControllerBase
    {
        [HttpGet(Router.SpecializationRouting.List)]
        [SwaggerOperation(Summary = "List all specializations", Description = "Get complete list of medical specializations with bilingual names")]
        public async Task<IActionResult> GetSpecializations()
        {
            var response = await _mediator.Send(new GetSpecializationsQuery());
            return NewResult(response);
        }

        [HttpGet(Router.SpecializationRouting.SubSpecializations)]
        [SwaggerOperation(Summary = "Get sub-specializations by specialization ID", Description = "Retrieve all sub-specializations for a specific medical specialization")]
        public async Task<IActionResult> GetSubSpecializationsBySpecID([FromRoute] GetSubSpecializationBySpecIDQuery query)
        {
            var response = await _mediator.Send(query);
            return NewResult(response);
        }

        [HttpPost(Router.SpecializationRouting.Create)]
        [Authorize(Roles = Roles.Admin)]
        [SwaggerOperation(Summary = "Create new specialization", Description = "Create a new medical specialization with bilingual names. Admin only")]
        public async Task<IActionResult> CreateSpecialization([FromForm] CreateSpecializationCommand request)
        {
            var response = await _mediator.Send(request);
            return NewResult(response);
        }

        [HttpPut(Router.SpecializationRouting.Update)]
        [Authorize(Roles = Roles.Admin)]
        [SwaggerOperation(Summary = "Update specialization", Description = "Update medical specialization details. Admin only")]
        public async Task<IActionResult> UpdateSpecialization([FromRoute] int Id, [FromBody] UpdateSpecializationCommand request)
        {
            request.Id = Id;
            var response = await _mediator.Send(request);
            return NewResult(response);
        }
    }
}
