using FluentValidation;
using Microsoft.Extensions.Localization;
using Vezeeta_Clone.Core.Features.Payments.Commands.Models;
using Vezeeta_Clone.Core.Resources;

namespace Vezeeta_Clone.Core.Features.Payments.Commands.Validators
{
    public class ConfirmPaymentValidator : AbstractValidator<ConfirmPaymentCommand>
    {
        private readonly IStringLocalizer<SharedResources> _localizer;
        public ConfirmPaymentValidator(IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer;
            ApplyValidationRules();
        }

        public void ApplyValidationRules()
        {
            RuleFor(x => x.PaymentMethodId)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
                .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required]);

            RuleFor(x => x.PaymentId)
              .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
              .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required]);
        }
    }
}
