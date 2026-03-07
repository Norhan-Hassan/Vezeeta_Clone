using FluentValidation;
using Microsoft.Extensions.Localization;
using Vezeeta_Clone.Core.Features.Auth.Commands.Models;
using Vezeeta_Clone.Core.Resources;

namespace Vezeeta_Clone.Core.Features.Auth.Commands.Validators
{
    public class DeleteRoleValidator : AbstractValidator<DeleteRoleCommand>
    {
        #region Fields

        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructors

        public DeleteRoleValidator(IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer;
            ApplyValidationRules();
            ApplyCustomValidationRules();
        }
        #endregion
        #region Functions
        public void ApplyValidationRules()
        {
            RuleFor(x => x.Id)
               .NotEmpty().WithMessage(x => _localizer[SharedResourcesKeys.NotEmpty])
               .NotNull().WithMessage(x => _localizer[SharedResourcesKeys.Required]);


        }
        public void ApplyCustomValidationRules()
        {

        }
        #endregion
    }
}

