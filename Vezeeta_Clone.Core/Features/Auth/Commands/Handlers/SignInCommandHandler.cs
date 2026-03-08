using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Vezeeta_Clone.Core.Bases;
using Vezeeta_Clone.Core.Features.Auth.Commands.Models;
using Vezeeta_Clone.Core.Resources;
using Vezeeta_Clone.Data.Entities;
using Vezeeta_Clone.Data.Results;
using Vezeeta_Clone.Service.Abstract;

namespace Vezeeta_Clone.Core.Features.Auth.Commands.Handlers
{
    public class SignInCommandHandler : ResponseHandler, IRequestHandler<SignInCommand, Response<JwtAuthResult>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IAuthenticationService _authenticationService;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public SignInCommandHandler(UserManager<ApplicationUser> userManager,
                                SignInManager<ApplicationUser> signInManager,
                                IAuthenticationService authenticationService,
                             IStringLocalizer<SharedResources> localizer) : base(localizer)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _authenticationService = authenticationService;
            _localizer = localizer;
        }

        public async Task<Response<JwtAuthResult>> Handle(SignInCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            var signInresult = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!signInresult.Succeeded)
            {
                return BadRequest<JwtAuthResult>(_localizer[SharedResourcesKeys.EmailOrPassNotExist]);
            }
            var result = await _authenticationService.GenerateJwtTokenAsync(user);

            return Success<JwtAuthResult>(result, message: _localizer[SharedResourcesKeys.SignInSuccess]);
        }
    }
}
