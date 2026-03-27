using FluentValidation;
using Microsoft.Extensions.Localization;
using Vezeeta_Clone.Core.Features.Auth.Queries.Models;
using Vezeeta_Clone.Core.Resources;

namespace Vezeeta_Clone.Core.Features.Auth.Queries.Validators
{
    public class ConfirmEmailQueryValidator : AbstractValidator<ConfirmEmailQuery>
    {
        private readonly IStringLocalizer<SharedResources> _localizer;
        public ConfirmEmailQueryValidator(IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer;
            ApplyValidationRules();
            ApplyCustomValidationRules();
        }

        public void ApplyValidationRules()
        {
            RuleFor(x => x.userEmail)
               .NotEmpty().WithMessage(x => _localizer[SharedResourcesKeys.NotEmpty])
               .NotNull().WithMessage(x => _localizer[SharedResourcesKeys.Required]);

            RuleFor(x => x.code)
                .NotEmpty().WithMessage(x => _localizer[SharedResourcesKeys.NotEmpty])
                .NotNull().WithMessage(x => _localizer[SharedResourcesKeys.Required]);
        }
        public void ApplyCustomValidationRules()
        {

        }
    }
}
