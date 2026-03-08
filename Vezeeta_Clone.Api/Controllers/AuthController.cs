using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vezeeta_Clone.Api.Base;
using Vezeeta_Clone.Core.Features.Auth.Commands.Models;
using Vezeeta_Clone.Core.Features.Auth.Queries.Models;
using Vezeeta_Clone.Data.AppMetaData;
using Vezeeta_Clone.Data.Commons;

namespace Vezeeta_Clone.Api.Controllers
{

    public class AuthController : AppControllerBase
    {
        [HttpPost(Router.AuthRouting.DoctorRegister)]
        public async Task<IActionResult> DoctorRegister([FromBody] RegisterDoctorCommand command)
        {
            var response = await _mediator.Send(command);
            return NewResult(response);
        }
        [HttpPost(Router.AuthRouting.PatientRegister)]
        public async Task<IActionResult> PatientRegister([FromBody] RegisterPatientCommand command)
        {
            var response = await _mediator.Send(command);
            return NewResult(response);
        }

        [HttpPost(Router.AuthRouting.SignIn)]
        public async Task<IActionResult> SignIn([FromForm] SignInCommand command)
        {
            var response = await _mediator.Send(command);
            return NewResult(response);
        }

        [HttpPost(Router.AuthRouting.RefreshToken)]
        public async Task<IActionResult> RefreshToken([FromForm] RefreshTokenCommand command)
        {
            var response = await _mediator.Send(command);
            return NewResult(response);
        }

        [HttpGet(Router.AuthRouting.ValidateToken)]
        public async Task<IActionResult> ValidateToken([FromQuery] AuthenticateUserQuery query)
        {
            var response = await _mediator.Send(query);
            return NewResult(response);
        }

        //--------------------------------Roles Management---------------------------------

        [Authorize(Roles = Roles.Admin)]
        [HttpPost(Router.AuthRouting.Add)]
        public async Task<IActionResult> AddRole([FromQuery] string roleName)
        {
            var response = await _mediator.Send(new AddRoleCommand { RoleName = roleName });
            return NewResult(response);
        }


        [Authorize(Roles = Roles.Admin)]
        [HttpPut(Router.AuthRouting.Update)]
        public async Task<IActionResult> UpdateRole([FromForm] UpdateRoleCommand command)
        {
            var response = await _mediator.Send(command);
            return NewResult(response);
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpDelete(Router.AuthRouting.Delete)]
        public async Task<IActionResult> DeleteRole([FromQuery] string id)
        {
            var response = await _mediator.Send(new DeleteRoleCommand { Id = id });
            return NewResult(response);
        }
    }
}
