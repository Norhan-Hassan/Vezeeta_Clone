using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Vezeeta_Clone.Core.Bases;
using Vezeeta_Clone.Core.Features.Auth.Commands.Models;
using Vezeeta_Clone.Core.Resources;
using Vezeeta_Clone.Data.Entities;
using Vezeeta_Clone.Service.Abstract;

namespace Vezeeta_Clone.Core.Features.Auth.Commands.Handlers
{
    public class SignInCommandHandler : ResponseHandler, IRequestHandler<SignInCommand, Response<string>>
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

        public async Task<Response<string>> Handle(SignInCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return BadRequest<string>(_localizer[SharedResourcesKeys.EmailOrPassNotExist]);
            }
            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!result.Succeeded || user == null)
            {
                return BadRequest<string>(_localizer[SharedResourcesKeys.EmailOrPassNotExist]);
            }
            var TokenResult = await _authenticationService.GenerateJwtTokenAsync(user);

            return Success<string>(TokenResult, message: _localizer[SharedResourcesKeys.SignInSuccess]);
        }
    }
}
