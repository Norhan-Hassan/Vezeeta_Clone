using FluentValidation;
using Microsoft.Extensions.Localization;
using Vezeeta_Clone.Core.Features.Reviews.Commands.Models;
using Vezeeta_Clone.Core.Resources;

namespace Vezeeta_Clone.Core.Features.Reviews.Commands.Validators
{
    public class MakeReviewCommandValidator : AbstractValidator<MakeReviewCommand>
    {
        private readonly IStringLocalizer<SharedResources> _localizer;
        public MakeReviewCommandValidator(IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer;
            ApplyValidationRules();
            ApplyCustomValidationRules();
        }

        public void ApplyValidationRules()
        {
            RuleFor(x => x.DoctorId)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
                .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required]);

            RuleFor(x => x.Rating)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
                .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required])
                .InclusiveBetween(0, 5).WithMessage(_localizer[SharedResourcesKeys.RatingRange]);

        }
        public void ApplyCustomValidationRules()
        {
            //RuleFor(x => x.IsAnonymous)
            //    .MustAsync(async (isAnonymous, cancellationToken) =>
            //    {
            //        isAnonymous = isAnonymous == null ? false : isAnonymous;
            //        return isAnonymous;
            //    });
        }
    }
}
