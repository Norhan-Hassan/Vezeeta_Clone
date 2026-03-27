using FluentValidation;
using Microsoft.Extensions.Localization;
using Vezeeta_Clone.Core.Features.Payments.Commands.Models;
using Vezeeta_Clone.Core.Resources;

namespace Vezeeta_Clone.Core.Features.Payments.Commands.Validators
{
    public class UpdateAppointmentAfterPaymentCommandValidator : AbstractValidator<UpdateAppointmentAfterPaymentCommand>
    {
        private readonly IStringLocalizer<SharedResources> _localizer;

        public UpdateAppointmentAfterPaymentCommandValidator(IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer;
            ApplyValidationRules();
        }

        public void ApplyValidationRules()
        {
            //RuleFor(x => x.IsPaid)
            //    .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
            //    .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required])
            //    .GetType().Equals(typeof(bool));

            RuleFor(x => x.PaymentId)
              .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
              .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required]);
        }
    }
}
