using FluentValidation;
using Vezeeta_Clone.Core.Features.ApplicationUserIdentity.Commands.Models;

namespace Vezeeta_Clone.Core.Features.ApplicationUserIdentity.Commands.Validators
{
    public class AddUserValidator : AbstractValidator<AddUserCommand>
    {
        #region Fields
        #endregion


        #region Constructor
        public AddUserValidator()
        {
            ApplyValidationRules();
            ApplyCustomValidationRules();
        }
        #endregion


        #region Methods
        public void ApplyValidationRules()
        {
            RuleFor(x => x.FirstName)
                    .NotEmpty().WithMessage("provide value in First Name")
                    .NotNull().WithMessage("Name shouldn't be null")
                    .MaximumLength(50).WithMessage("length should be 50 char maximum");

            RuleFor(x => x.LastName)
                    .NotEmpty().WithMessage("provide value in Last Name")
                    .NotNull().WithMessage("Last Name shouldn't be null")
                    .MaximumLength(50).WithMessage("length should be 50 char maximum");

            RuleFor(x => x.UserName)
                   .NotEmpty().WithMessage("provide value in User Name")
                   .NotNull().WithMessage("User Name shouldn't be null")
                   .MaximumLength(50).WithMessage("length should be 50 char maximum");

            RuleFor(x => x.Email)
                  .NotEmpty().WithMessage("provide value in Email")
                  .NotNull().WithMessage("Email shouldn't be null");

            RuleFor(x => x.Password)
                  .NotEmpty().WithMessage("provide value in Password")
                  .NotNull().WithMessage("Password shouldn't be null");

            RuleFor(x => x.ConfirmPassword)
                  .NotEmpty().WithMessage("provide value in Confirm Password")
                  .NotNull().WithMessage("Confirm Password shouldn't be null")
                  .Equal(x => x.Password).WithMessage("Password and confirm password don't match");
        }

        public void ApplyCustomValidationRules()
        {

        }
        #endregion

    }
}
