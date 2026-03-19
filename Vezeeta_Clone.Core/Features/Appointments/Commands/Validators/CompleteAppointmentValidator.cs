using FluentValidation;
using Microsoft.Extensions.Localization;
using Vezeeta_Clone.Core.Features.Appointments.Commands.Models;
using Vezeeta_Clone.Core.Resources;

namespace Vezeeta_Clone.Core.Features.Appointments.Commands.Validators
{
    public class CompleteAppointmentValidator : AbstractValidator<CompleteAppointmentCommand>
    {
        private readonly IStringLocalizer<SharedResources> _localizer;

        public CompleteAppointmentValidator(IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer;
            ApplyValidationRules();
            ApplyCustomValidationRules();
        }



        public void ApplyValidationRules()
        {
            RuleFor(x => x.AppointmentId)
               .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
               .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required]);
        }

        public void ApplyCustomValidationRules()
        {

        }
    }
}
