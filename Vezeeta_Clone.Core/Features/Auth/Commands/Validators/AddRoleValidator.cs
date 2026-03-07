using FluentValidation;
using Microsoft.Extensions.Localization;
using Vezeeta_Clone.Core.Features.Auth.Commands.Models;
using Vezeeta_Clone.Core.Resources;
using Vezeeta_Clone.Service.Abstract;

namespace Vezeeta_Clone.Core.Features.Auth.Commands.Validators
{
    public class AddRoleValidator : AbstractValidator<AddRoleCommand>
    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IAutherizationService _autherzationService;

        #endregion

        #region Constructors
        public AddRoleValidator(IStringLocalizer<SharedResources> localizer,
                                IAutherizationService autherzationService)
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
            RuleFor(x => x.RoleName)
                .NotEmpty().WithMessage(x => _localizer[SharedResourcesKeys.NotEmpty])
                .NotNull().WithMessage(x => _localizer[SharedResourcesKeys.Required]);
        }
        public void ApplyCustomValidationRules()
        {
            RuleFor(x => x.RoleName)
               .MustAsync(async (Key, CancellationToken) => !await _autherzationService.IsRoleExist(Key))
               .WithMessage(x => _localizer[SharedResourcesKeys.IsExist]);
        }
        #endregion
    }
}
