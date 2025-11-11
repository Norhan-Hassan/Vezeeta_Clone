using Microsoft.AspNetCore.Mvc;
using Vezeeta_Clone.Api.Base;
using Vezeeta_Clone.Core.Features.Doctor.Queries.Models;
using Vezeeta_Clone.Data.AppMetaData;

namespace Vezeeta_Clone.Api.Controllers
{
    [ApiController]
    public class DoctorController : AppControllerBase
    {

        [HttpGet(Router.DoctorRouting.GetById)]
        public async Task<ActionResult> GetDoctorByID([FromRoute] string Id)
        {
            var response = await _mediator.Send(new GetDoctorByIdQuery() { Id = Id });
            return NewResult(response);
        }

    }
}
