using FluentValidation;
using Microsoft.Extensions.Localization;
using Vezeeta_Clone.Core.Features.Appointments.Commands.Models;
using Vezeeta_Clone.Core.Resources;
using Vezeeta_Clone.Service.AppUserAuthServices.Abstract;

namespace Vezeeta_Clone.Core.Features.Appointments.Commands.Validators
{
    public class BookAppointmentValidator : AbstractValidator<BookAppointmentCommand>
    {
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly ICurrentUserService _currentUserService;
        public BookAppointmentValidator(IStringLocalizer<SharedResources> localizer, ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
            _localizer = localizer;
            ApplyValidationRules();
            ApplyCustomValidationRules();
        }

        public void ApplyValidationRules()
        {

            RuleFor(x => x.SlotId)
               .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
               .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required]);

            RuleFor(x => x.ActualPatientName)
              .MaximumLength(100)
              .When(x => !string.IsNullOrEmpty(x.ActualPatientName));

            RuleFor(x => x.ActualPatientPhone)
                .Matches(@"^01[0|1|2|5]\d{8}$").WithMessage(x => _localizer[SharedResourcesKeys.InvalidPhoneNumber])
                .When(x => !string.IsNullOrEmpty(x.ActualPatientPhone));

        }

        public void ApplyCustomValidationRules()
        {
            //when not providing actual patient name or phone, get them from the current user 
            RuleFor(x => x)
            .MustAsync(async (command, cancellation) =>
            {
                var patient = await _currentUserService.GetCurrentUserAsync();

                command.ActualPatientName = !string.IsNullOrEmpty(command.ActualPatientName)
                    ? command.ActualPatientName
                    : patient.FirstName + " " + patient.LastName;

                command.ActualPatientPhone = !string.IsNullOrEmpty(command.ActualPatientPhone)
                     ? command.ActualPatientPhone
                     : patient.PhoneNumber;

                return true;
            });
        }
    }
}
