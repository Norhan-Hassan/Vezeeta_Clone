using FluentValidation;
using Microsoft.Extensions.Localization;
using Vezeeta_Clone.Core.Features.Auth.Commands.Models;
using Vezeeta_Clone.Core.Resources;
using Vezeeta_Clone.Service.Abstract;

namespace Vezeeta_Clone.Core.Features.Auth.Commands.Validators
{
    public class UpdateRoleValidator : AbstractValidator<UpdateRoleCommand>
    {
        #region Fields

        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IAutherizationService _autherzationService;
        #endregion

        #region Constructors
        public UpdateRoleValidator(IStringLocalizer<SharedResources> localizer, IAutherizationService autherzationService)

        {
            _localizer = localizer;
            _autherzationService = autherzationService;
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

            RuleFor(x => x.RoleName)
                .NotEmpty().WithMessage(x => _localizer[SharedResourcesKeys.NotEmpty])
                .NotNull().WithMessage(x => _localizer[SharedResourcesKeys.Required]);
        }
        public void ApplyCustomValidationRules()
        {
            // Custom validation to check if the role exists before updating
            //and also to check if the new role name doesn't already exist to prevent duplicates
        }
        #endregion

    }
}
