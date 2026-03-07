using FluentValidation;
using Microsoft.Extensions.Localization;
using Vezeeta_Clone.Core.Features.Auth.Commands.Models;
using Vezeeta_Clone.Core.Resources;
using Vezeeta_Clone.Service.Abstract;

namespace Vezeeta_Clone.Core.Features.Auth.Commands.Validators
{
    public class RegisterDoctorValidator : AbstractValidator<RegisterDoctorCommand>
    {
        #region Fields

        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IAuthenticationService _authenticationService;
        #endregion

        #region Constructors
        public RegisterDoctorValidator(IStringLocalizer<SharedResources> localizer,
                                    IAuthenticationService authenticationService)
        {
            _localizer = localizer;
            _authenticationService = authenticationService;
            ApplyValidationRules();
            ApplyCustomValidationRules();
        }
        #endregion
        #region Functions
        public void ApplyValidationRules()
        {
            RuleFor(x => x.Title)
               .NotEmpty().WithMessage(x => _localizer[SharedResourcesKeys.NotEmpty])
               .NotNull().WithMessage(x => _localizer[SharedResourcesKeys.Required]);

            RuleFor(x => x.ExperienceInYears)
               .NotEmpty().WithMessage(x => _localizer[SharedResourcesKeys.NotEmpty])
               .NotNull().WithMessage(x => _localizer[SharedResourcesKeys.Required]);

            RuleFor(x => x.Description)
               .NotEmpty().WithMessage(x => _localizer[SharedResourcesKeys.NotEmpty])
               .NotNull().WithMessage(x => _localizer[SharedResourcesKeys.Required]);

            RuleFor(x => x.FirstName)
               .NotEmpty().WithMessage(x => _localizer[SharedResourcesKeys.NotEmpty])
               .NotNull().WithMessage(x => _localizer[SharedResourcesKeys.Required]);

            RuleFor(x => x.LastName)
               .NotEmpty().WithMessage(x => _localizer[SharedResourcesKeys.NotEmpty])
               .NotNull().WithMessage(x => _localizer[SharedResourcesKeys.Required]);

            RuleFor(x => x.Email)
               .NotEmpty().WithMessage(x => _localizer[SharedResourcesKeys.NotEmpty])
               .NotNull().WithMessage(x => _localizer[SharedResourcesKeys.Required])
               .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$").WithMessage(x => _localizer[SharedResourcesKeys.EmailFormat]);

            RuleFor(x => x.PhoneNumber)
               .NotEmpty().WithMessage(x => _localizer[SharedResourcesKeys.NotEmpty])
               .NotNull().WithMessage(x => _localizer[SharedResourcesKeys.Required])
               .Matches(@"^01[0|1|2|5]\d{8}$").WithMessage(x => _localizer[SharedResourcesKeys.InvalidPhoneNumber]);

            RuleFor(x => x.gender)
               .NotEmpty().WithMessage(x => _localizer[SharedResourcesKeys.NotEmpty]);

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
        public void ApplyCustomValidationRules()
        {
            RuleFor(x => x.Email)
                .MustAsync(async (email, cancellation) => !await _authenticationService.UserExistsByEmailAsync(email))
                .WithMessage(x => _localizer[SharedResourcesKeys.EmailAlreadyExists]);

            RuleFor(x => x.UserName)
              .MustAsync(async (userName, cancellation) => !await _authenticationService.UserExistsByUserNameAsync(userName))
              .WithMessage(x => _localizer[SharedResourcesKeys.UserNameAlreadyExists]);


        }
        #endregion
    }
}
