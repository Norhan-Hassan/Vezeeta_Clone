using FluentValidation;
using Microsoft.Extensions.Localization;
using Vezeeta_Clone.Core.Features.Reviews.Commands.Models;
using Vezeeta_Clone.Core.Resources;

namespace Vezeeta_Clone.Core.Features.Reviews.Commands.Validators
{
    public class DeleteReviewCommandValidator : AbstractValidator<DeleteReviewCommand>
    {
        private readonly IStringLocalizer<SharedResources> _localizer;
        public DeleteReviewCommandValidator(IStringLocalizer<SharedResources> localizer)
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



        }
        public void ApplyCustomValidationRules()
        {

        }
    }
}
