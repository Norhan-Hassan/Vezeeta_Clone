using FluentValidation;
using Microsoft.Extensions.Localization;
using Vezeeta_Clone.Core.Features.Reviews.Commands.Models;
using Vezeeta_Clone.Core.Resources;

namespace Vezeeta_Clone.Core.Features.Reviews.Commands.Validators
{
    public class UpdateReviewCommandValidator : AbstractValidator<UpdateReviewCommand>
    {
        private readonly IStringLocalizer<SharedResources> _localizer;
        public UpdateReviewCommandValidator(IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer;
            ApplyValidationRules();
            ApplyCustomValidationRules();
        }

        public void ApplyValidationRules()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
                .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required]);

            RuleFor(x => x.Rating)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
                .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required])
                .InclusiveBetween(0, 5).WithMessage(_localizer[SharedResourcesKeys.RatingRange]);
        }
        public void ApplyCustomValidationRules()
        {

        }
    }
}
