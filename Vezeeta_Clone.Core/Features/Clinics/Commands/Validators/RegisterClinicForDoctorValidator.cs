using FluentValidation;
using Microsoft.Extensions.Localization;
using Vezeeta_Clone.Core.Features.Clinics.Commands.Models;
using Vezeeta_Clone.Core.Resources;
using Vezeeta_Clone.Service.Abstract;

namespace Vezeeta_Clone.Core.Features.Clinics.Commands.Validators
{
    public class RegisterClinicForDoctorValidator : AbstractValidator<RegisterClinicForDoctorCommand>
    {
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IClinicService _clinicService;
        public RegisterClinicForDoctorValidator(IStringLocalizer<SharedResources> localizer, IClinicService clinicService)
        {
            _localizer = localizer;
            _clinicService = clinicService;
            ApplyValidationRules();
            ApplyCustomValidationRules();
        }


        public void ApplyValidationRules()
        {
            RuleFor(x => x.ClinicName)
               .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
               .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required]);
            RuleFor(x => x.Address)
               .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
               .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required]);
            RuleFor(x => x.RegionId)
               .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
               .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required]);
            RuleFor(x => x.ClinicLocation)
              .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
              .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required]);

            RuleFor(x => x.PhoneNumber)
             .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
             .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required])
             .Matches(@"^01[0|1|2|5]\d{8}$").WithMessage(x => _localizer[SharedResourcesKeys.InvalidPhoneNumber]);
        }
        public void ApplyCustomValidationRules()
        {

        }
    }
}

