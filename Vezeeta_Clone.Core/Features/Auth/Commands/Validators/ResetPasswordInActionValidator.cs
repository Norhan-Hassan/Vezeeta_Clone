using FluentValidation;
using Microsoft.Extensions.Localization;
using Vezeeta_Clone.Core.Features.Auth.Commands.Models;
using Vezeeta_Clone.Core.Resources;

namespace Vezeeta_Clone.Core.Features.Auth.Commands.Validators
{
    public class ResetPasswordInActionValidator : AbstractValidator<ResetPasswordInActionCommand>
    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructor
        public ResetPasswordInActionValidator(IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer;
            ApplyValidationRules();
        }
        #endregion

        #region Functions
        public void ApplyValidationRules()
        {
            RuleFor(x => x.Email)
               .NotEmpty().WithMessage(x => _localizer[SharedResourcesKeys.NotEmpty])
               .NotNull().WithMessage(x => _localizer[SharedResourcesKeys.Required]);

            RuleFor(x => x.Password)
               .NotEmpty().WithMessage(x => _localizer[SharedResourcesKeys.NotEmpty])
               .NotNull().WithMessage(x => _localizer[SharedResourcesKeys.Required])
               .MinimumLength(8).WithMessage(x => _localizer[SharedResourcesKeys.PasswordMinLength])
               .Matches(@"^(?=.*[A-Za-z])(?=.*\d).{8,}$")
               .WithMessage(x => _localizer[SharedResourcesKeys.PasswordCriteria]);

            RuleFor(x => x.ConfirmPassword)
               .Cascade(CascadeMode.Continue)
               .NotEmpty().WithMessage(x => _localizer[SharedResourcesKeys.NotEmpty])
               .NotNull().WithMessage(x => _localizer[SharedResourcesKeys.Required])
               .Equal(x => x.Password)
               .When(x => !string.IsNullOrEmpty(x.Password))
               .WithMessage(x => _localizer[SharedResourcesKeys.PassMatchConfirm]);

        }
        #endregion

    }
}