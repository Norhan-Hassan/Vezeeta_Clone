using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
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
        [SwaggerOperation(Summary = "Register doctor account", Description = "Create new doctor account with email, password and profile information")]
        public async Task<IActionResult> DoctorRegister([FromBody] RegisterDoctorCommand command)
        {
            var response = await _mediator.Send(command);
            return NewResult(response);
        }

        [HttpPost(Router.AuthRouting.PatientRegister)]
        [SwaggerOperation(Summary = "Register patient account", Description = "Create new patient account with email, password and personal information")]
        public async Task<IActionResult> PatientRegister([FromBody] RegisterPatientCommand command)
        {
            var response = await _mediator.Send(command);
            return NewResult(response);
        }

        [HttpPost(Router.AuthRouting.SignIn)]
        [SwaggerOperation(Summary = "User sign in", Description = "Authenticate user and retrieve JWT access and refresh tokens")]
        public async Task<IActionResult> SignIn([FromBody] SignInCommand command)
        {
            var response = await _mediator.Send(command);
            return NewResult(response);
        }

        [Authorize]
        [HttpPost(Router.AuthRouting.ChangePassword)]
        [SwaggerOperation(Summary = "Change password", Description = "Update user password for authenticated users", OperationId = "ResetPassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand command)
        {
            var response = await _mediator.Send(command);
            return NewResult(response);
        }

        [HttpPost(Router.AuthRouting.ResetPassword)]
        [SwaggerOperation(Summary = "Request password reset", Description = "Send password reset code to user email", OperationId = "ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromQuery] ResetPasswordCommand command)
        {
            var response = await _mediator.Send(command);
            return NewResult(response);
        }

        [HttpGet(Router.AuthRouting.CheckResetPassword)]
        [SwaggerOperation(Summary = "Verify reset password code", Description = "Validate password reset code sent to user email", OperationId = "ResetPassword")]
        public async Task<IActionResult> CheckResetPassword([FromQuery] ResetPasswordQuery query)
        {
            var response = await _mediator.Send(query);
            return NewResult(response);
        }

        [HttpPost(Router.AuthRouting.ResetPasswordInAction)]
        [SwaggerOperation(Summary = "Reset password with code", Description = "Complete password reset using verification code", OperationId = "ResetPassword")]
        public async Task<IActionResult> ResetPasswordInAction([FromBody] ResetPasswordInActionCommand command)
        {
            var response = await _mediator.Send(command);
            return NewResult(response);
        }

        [HttpGet(Router.AuthRouting.ConfirmEmail)]
        [SwaggerOperation(Summary = "Confirm email address", Description = "Activate user account by confirming email address")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] ConfirmEmailQuery query)
        {
            var response = await _mediator.Send(query);
            return NewResult(response);
        }

        [HttpPost(Router.AuthRouting.RefreshToken)]
        [SwaggerOperation(Summary = "Refresh access token", Description = "Generate new access token using refresh token")]
        public async Task<IActionResult> RefreshToken([FromForm] RefreshTokenCommand command)
        {
            var response = await _mediator.Send(command);
            return NewResult(response);
        }

        [HttpGet(Router.AuthRouting.ValidateToken)]
        [SwaggerOperation(Summary = "Validate JWT token", Description = "Check if JWT token is valid and not expired")]
        public async Task<IActionResult> ValidateToken([FromQuery] AuthenticateUserQuery query)
        {
            var response = await _mediator.Send(query);
            return NewResult(response);
        }
    }
}
