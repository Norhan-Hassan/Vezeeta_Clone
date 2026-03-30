using FluentValidation;
using Microsoft.Extensions.Localization;
using Vezeeta_Clone.Core.Features.Clinics.Commands.Models;
using Vezeeta_Clone.Core.Resources;

namespace Vezeeta_Clone.Core.Features.Clinics.Commands.Validators
{
    public class AddClinicImagesValidator : AbstractValidator<AddClinicImagesCommand>
    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructor
        public AddClinicImagesValidator(IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer;
            ApplyValidationRules();
            ApplyCustomValidationRules();
        }
        #endregion

        #region Functions


        public void ApplyValidationRules()
        {
            RuleFor(x => x.ClinicId)
               .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
               .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required]);

        }
        public void ApplyCustomValidationRules()
        {
            RuleFor(x => x)
                .MustAsync(async (model, CancellationToken) => model.Images.Count > 0)
                .WithMessage(_localizer[SharedResourcesKeys.AtLeastOneImage]);

            RuleFor(x => x)
               .MustAsync(async (model, CancellationToken) => model.Images.Count < 5)
               .WithMessage(_localizer[SharedResourcesKeys.AtMostFiveImages]);
        }
        #endregion
    }
}

