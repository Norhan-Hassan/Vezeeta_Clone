using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Vezeeta_Clone.Core.Bases;
using Vezeeta_Clone.Core.Features.Auth.Commands.Models;
using Vezeeta_Clone.Core.Resources;
using Vezeeta_Clone.Data.Entities;
using Vezeeta_Clone.Service.AppUserAuthServices.Abstract;

namespace Vezeeta_Clone.Core.Features.Auth.Commands.Handlers
{
    public class ChangePasswordCommandHandler : ResponseHandler, IRequestHandler<ChangePasswordCommand, Response<string>>
    {
        #region Fields
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICurrentUserService _currentUserService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion
        #region Constructor
        public ChangePasswordCommandHandler(UserManager<ApplicationUser> userManager,
                                            IStringLocalizer<SharedResources> localizer,
                                            ICurrentUserService currentUserService) : base(localizer)
        {
            _userManager = userManager;
            _currentUserService = currentUserService;
            _localizer = localizer;
        }
        #endregion

        #region Functions
        public async Task<Response<string>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _currentUserService.GetCurrentUserAsync();
            if (user != null)
            {
                var validPass = await _userManager.CheckPasswordAsync(user, request.CurrentPassword);
                if (validPass)
                {
                    var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);

                    if (result.Succeeded)
                    {
                        return Success<string>(null, message: _localizer[SharedResourcesKeys.PasswordChangedSuccess]);
                    }
                    else
                    {
                        return BadRequest<string>(_localizer[SharedResourcesKeys.FailedToChangePassword]);
                    }
                }
                else
                {
                    return BadRequest<string>(_localizer[SharedResourcesKeys.CurrentPasswordIsIncorrect]);
                }
            }
            return NotFound<string>(_localizer[SharedResourcesKeys.UserNotFound]);
        }
        #endregion
    }
}
