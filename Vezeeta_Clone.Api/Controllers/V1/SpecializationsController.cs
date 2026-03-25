
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Vezeeta_Clone.Api.Base;
using Vezeeta_Clone.Core.Features.Specializations.Commands.Models;
using Vezeeta_Clone.Core.Features.Specializations.Queries.Models;
using Vezeeta_Clone.Data.AppMetaData;

namespace Vezeeta_Clone.Api.Controllers.V1
{
    [ApiVersion("1")]
    public class SpecializationsController : AppControllerBase
    {


        [HttpGet(Router.SpecializationRouting.List)]
        public async Task<IActionResult> GetSpecializations()
        {
            var response = await _mediator.Send(new GetSpecializationsQuery());
            return NewResult(response);
        }

        [HttpGet(Router.SpecializationRouting.SubSpecializations)]
        public async Task<IActionResult> GetSubSpecializationsBySpecID([FromRoute] GetSubSpecializationBySpecIDQuery query)
        {
            var response = await _mediator.Send(query);
            return NewResult(response);
        }

        [HttpPost(Router.SpecializationRouting.Create)]
        public async Task<IActionResult> CreateSpecialization([FromForm] CreateSpecializationCommand request)
        {
            var response = await _mediator.Send(request);
            return NewResult(response);
        }


        [HttpPut(Router.SpecializationRouting.Update)]
        public async Task<IActionResult> UpdateSpecialization([FromRoute] int Id, [FromBody] UpdateSpecializationCommand request)
        {
            request.Id = Id;
            var response = await _mediator.Send(request);
            return NewResult(response);

        }
    }
}
