using FluentValidation;
using Microsoft.Extensions.Localization;
using Vezeeta_Clone.Core.Features.Doctors.Commands.Models;
using Vezeeta_Clone.Core.Resources;

namespace Vezeeta_Clone.Core.Features.Doctors.Commands.Validators
{
    public class CompleteDoctorInfoCommandValidator : AbstractValidator<CompleteDoctorInfoCommand>
    {
        private readonly IStringLocalizer<SharedResources> _localizer;
        public CompleteDoctorInfoCommandValidator(IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer;
            ApplyValidationRules();
            ApplyCustomValidationRules();
        }

        public void ApplyValidationRules()
        {
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
                .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required]);

        }
        public void ApplyCustomValidationRules()
        {
            RuleFor(x => x.SubSpecializations)
                .Must(subSpecializations => subSpecializations!.Length <= 3 && subSpecializations!.Length >= 0);

        }
    }
}
