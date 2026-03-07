using FluentValidation;
using Microsoft.Extensions.Localization;
using Vezeeta_Clone.Core.Features.Specializations.Commands.Models;
using Vezeeta_Clone.Core.Resources;
using Vezeeta_Clone.Service.Abstract;

namespace Vezeeta_Clone.Core.Features.Specializations.Commands.Validators
{
    public class CreateSpecializationValidator : AbstractValidator<CreateSpecializationCommand>
    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly ISpecializationService _specializationService;
        #endregion

        #region Constructors
        public CreateSpecializationValidator(IStringLocalizer<SharedResources> localizer, ISpecializationService specializationService)
        {
            _localizer = localizer;
            _specializationService = specializationService;
            ApplyValidationRules();
            ApplyCustomValidationRules();
        }
        #endregion

        #region Functions
        public void ApplyValidationRules()
        {
            RuleFor(x => x.Name)
               .NotEmpty().WithMessage(x => _localizer[SharedResourcesKeys.NotEmpty])
               .NotNull().WithMessage(x => _localizer[SharedResourcesKeys.Required]);
        }
        public void ApplyCustomValidationRules()
        {
            RuleFor(x => x.Name)
                .MustAsync(async (name, cancellation) => !await _specializationService.IsSpecializationExist(name))
            .WithMessage(x => _localizer[SharedResourcesKeys.IsExist]);
        }
        #endregion

    }
}
