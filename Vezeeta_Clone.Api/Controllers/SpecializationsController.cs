
using Microsoft.AspNetCore.Mvc;
using Vezeeta_Clone.Api.Base;
using Vezeeta_Clone.Core.Features.Specializations.Commands.Models;
using Vezeeta_Clone.Core.Features.Specializations.Queries.Models;
using Vezeeta_Clone.Data.AppMetaData;

namespace Vezeeta_Clone.Api.Controllers
{

    public class SpecializationsController : AppControllerBase
    {

        [HttpPost(Router.SpecializationRouting.Create)]
        public async Task<IActionResult> CreateSpecialization([FromForm] CreateSpecializationCommand request)
        {
            var response = await _mediator.Send(request);
            return NewResult(response);
        }


        [HttpPut(Router.SpecializationRouting.Update)]
        public async Task<IActionResult> CreateSpecialization([FromForm] UpdateSpecializationCommand request)
        {
            var response = await _mediator.Send(request);
            return NewResult(response);

        }


        [HttpGet(Router.SpecializationRouting.SubSpecializations)]
        public async Task<IActionResult> GetSubSpecializationsBySpecID([FromRoute] GetSubSpecializationBySpecIDQuery query)
        {
            var response = await _mediator.Send(query);
            return NewResult(response);
        }
    }
}
