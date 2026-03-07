using FluentValidation;
using Microsoft.Extensions.Localization;
using Vezeeta_Clone.Core.Features.Auth.Commands.Models;
using Vezeeta_Clone.Core.Resources;

namespace Vezeeta_Clone.Core.Features.Auth.Commands.Validators
{
    public class SignInValidator : AbstractValidator<SignInCommand>
    {
        private readonly IStringLocalizer<SharedResources> _localizer;
        public SignInValidator(IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer;
            ApplyValidationRules();
            ApplyCustomValidationRules();
        }


        public void ApplyValidationRules()
        {
            RuleFor(x => x.Email)
               .NotEmpty().WithMessage(x => _localizer[SharedResourcesKeys.NotEmpty])
               .NotNull().WithMessage(x => _localizer[SharedResourcesKeys.Required]);

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage(x => _localizer[SharedResourcesKeys.NotEmpty])
                .NotNull().WithMessage(x => _localizer[SharedResourcesKeys.Required]);
        }
        public void ApplyCustomValidationRules()
        {

        }
    }
}
