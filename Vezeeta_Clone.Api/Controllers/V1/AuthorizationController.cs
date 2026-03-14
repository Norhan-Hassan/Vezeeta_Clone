using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vezeeta_Clone.Api.Base;
using Vezeeta_Clone.Core.Features.Auth.Commands.Models;
using Vezeeta_Clone.Data.AppMetaData;
using Vezeeta_Clone.Data.Commons;

namespace Vezeeta_Clone.Api.Controllers.V1
{
    [ApiVersion("1")]
    [Authorize(Roles = Roles.Admin)]
    public class AuthorizationController : AppControllerBase
    {


        [HttpPost(Router.AuthRouting.Add)]
        public async Task<IActionResult> AddRole([FromQuery] string roleName)
        {
            var response = await _mediator.Send(new AddRoleCommand { RoleName = roleName });
            return NewResult(response);
        }



        [HttpPut(Router.AuthRouting.Update)]
        public async Task<IActionResult> UpdateRole([FromForm] UpdateRoleCommand command)
        {
            var response = await _mediator.Send(command);
            return NewResult(response);
        }


        [HttpDelete(Router.AuthRouting.Delete)]
        public async Task<IActionResult> DeleteRole([FromQuery] string id)
        {
            var response = await _mediator.Send(new DeleteRoleCommand { Id = id });
            return NewResult(response);
        }
    }
}
