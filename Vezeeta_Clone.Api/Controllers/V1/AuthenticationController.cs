using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vezeeta_Clone.Api.Base;
using Vezeeta_Clone.Core.Features.Auth.Commands.Models;
using Vezeeta_Clone.Core.Features.Auth.Queries.Models;
using Vezeeta_Clone.Data.AppMetaData;

namespace Vezeeta_Clone.Api.Controllers.V1
{
    [ApiVersion("1")]
    public class AuthenticationController : AppControllerBase
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
        public async Task<IActionResult> SignIn([FromBody] SignInCommand command)
        {
            var response = await _mediator.Send(command);
            return NewResult(response);
        }
        [Authorize]
        [HttpPost(Router.AuthRouting.ChangePassword)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand command)
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


    }
}
