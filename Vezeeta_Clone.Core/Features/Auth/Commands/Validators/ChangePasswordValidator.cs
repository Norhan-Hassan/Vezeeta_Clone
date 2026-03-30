using FluentValidation;
using Microsoft.Extensions.Localization;
using Vezeeta_Clone.Core.Features.Auth.Commands.Models;
using Vezeeta_Clone.Core.Resources;
using Vezeeta_Clone.Service.Abstract;

namespace Vezeeta_Clone.Core.Features.Auth.Commands.Validators
{
    public class ChangePasswordValidator : AbstractValidator<ChangePasswordCommand>
    {
        #region Fields
        private readonly IAuthenticationService _authenticationService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructor
        public ChangePasswordValidator(IAuthenticationService authenticationService, IStringLocalizer<SharedResources> localizer)
        {
            _authenticationService = authenticationService;
            _localizer = localizer;
            ApplyValidationRules();
            ApplyCustomValidationRules();
        }
        #endregion

        #region Functions
        public void ApplyValidationRules()
        {


            RuleFor(x => x.CurrentPassword)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
                .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required]);

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage(x => _localizer[SharedResourcesKeys.NotEmpty])
                .NotNull().WithMessage(x => _localizer[SharedResourcesKeys.Required])
                .MinimumLength(8).WithMessage(x => _localizer[SharedResourcesKeys.PasswordMinLength]).
                Matches(@"^(?=.*[A-Za-z])(?=.*\d).{8,}$")
                .WithMessage(x => _localizer[SharedResourcesKeys.PasswordCriteria]);

            RuleFor(x => x.ConfirmNewPassword)
               .Cascade(CascadeMode.Continue)
               .NotEmpty().WithMessage(x => _localizer[SharedResourcesKeys.NotEmpty])
               .NotNull().WithMessage(x => _localizer[SharedResourcesKeys.Required])
               .Equal(x => x.NewPassword)
               .When(x => !string.IsNullOrEmpty(x.NewPassword))
               .WithMessage(x => _localizer[SharedResourcesKeys.PassMatchConfirm]);


        }

        public void ApplyCustomValidationRules()
        {

        }
        #endregion

    }
}
