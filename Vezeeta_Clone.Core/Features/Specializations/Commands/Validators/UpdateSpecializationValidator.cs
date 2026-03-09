using FluentValidation;
using Microsoft.Extensions.Localization;
using Vezeeta_Clone.Core.Features.Specializations.Commands.Models;
using Vezeeta_Clone.Core.Resources;
using Vezeeta_Clone.Service.Abstract;

namespace Vezeeta_Clone.Core.Features.Specializations.Commands.Validators
{
    public class UpdateSpecializationValidator : AbstractValidator<UpdateSpecializationCommand>
    {

        #region Fields
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly ISpecializationService _specializationService;
        #endregion


        #region Constructors
        public UpdateSpecializationValidator(IStringLocalizer<SharedResources> localizer,
                                                  ISpecializationService specializationService)
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
            RuleFor(x => x.Id)
              .NotEmpty().WithMessage(x => _localizer[SharedResourcesKeys.NotEmpty])
              .NotNull().WithMessage(x => _localizer[SharedResourcesKeys.Required]);


            RuleFor(x => x.NameAr)
               .NotEmpty().WithMessage(x => _localizer[SharedResourcesKeys.NotEmpty])
               .NotNull().WithMessage(x => _localizer[SharedResourcesKeys.Required]);

            RuleFor(x => x.NameEn)
              .NotEmpty().WithMessage(x => _localizer[SharedResourcesKeys.NotEmpty])
              .NotNull().WithMessage(x => _localizer[SharedResourcesKeys.Required]);
        }
        public void ApplyCustomValidationRules()
        {
            RuleFor(x => x)
               .MustAsync(async (model, cancellation) => !await _specializationService.IsSpecializationExist(specializationNameAr: model.NameAr, specializationNameEn: model.NameEn, currentId: model.Id))
           .WithMessage(x => _localizer[SharedResourcesKeys.IsExist]);
        }
        #endregion
    }
}
