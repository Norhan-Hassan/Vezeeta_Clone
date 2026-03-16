using FluentValidation;
using Microsoft.Extensions.Localization;
using Vezeeta_Clone.Core.Features.Scheduling.Commands.Models;
using Vezeeta_Clone.Core.Resources;
using Vezeeta_Clone.Service.Abstract;

namespace Vezeeta_Clone.Core.Features.Scheduling.Commands.Validators
{
    public class SetDoctorScheduleCommandValidator : AbstractValidator<SetDoctorScheduleCommand>
    {
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IScheduleService _scheduleService;
        public SetDoctorScheduleCommandValidator(IStringLocalizer<SharedResources> localizer,

                                                 IScheduleService scheduleService)
        {
            _localizer = localizer;
            _scheduleService = scheduleService;

            ApplyValidationRules();
            ApplyCustomValidationRules();
        }


        public void ApplyValidationRules()
        {
            RuleFor(x => x.StartTime)
               .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
               .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required]);
            RuleFor(x => x.EndTime)
               .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
               .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required]);
            RuleFor(x => x.Duration)
               .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
               .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required]);
            RuleFor(x => x.type)
               .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
               .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required]);
        }
        public void ApplyCustomValidationRules()
        {
            RuleFor(x => x.EndTime)
                .GreaterThan(x => x.StartTime)
                .WithMessage(_localizer[SharedResourcesKeys.EndTimeMustBeGreaterThanStartTime]);

            RuleFor(x => x.Duration)
                .GreaterThanOrEqualTo(5)
                .WithMessage(_localizer[SharedResourcesKeys.DurationMustBeGreaterThan5]);



        }
    }
}
