
using Microsoft.AspNetCore.Mvc;
using Vezeeta_Clone.Api.Base;
using Vezeeta_Clone.Core.Features.Specializations.Commands.Models;
using Vezeeta_Clone.Data.AppMetaData;

namespace Vezeeta_Clone.Api.Controllers
{

    public class SpecializationsController : AppControllerBase
    {

        [HttpPost(Router.SpecializationRouting.Create)]
        public async Task<IActionResult> CreateSpecialization([FromForm] CreateSpecializationCommand request)
        {
            var response = await _mediator.Send(new CreateSpecializationCommand { Name = request.Name, Description = request.Description });
            return NewResult(response);
        }
    }
}
