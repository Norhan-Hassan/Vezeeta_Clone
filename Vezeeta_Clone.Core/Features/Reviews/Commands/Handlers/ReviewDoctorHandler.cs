using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using Vezeeta_Clone.Core.Bases;
using Vezeeta_Clone.Core.Features.Reviews.Commands.Models;
using Vezeeta_Clone.Core.Resources;
using Vezeeta_Clone.Data.Entities;
using Vezeeta_Clone.Service.Abstract;
using Vezeeta_Clone.Service.AppUserAuthServices.Abstract;

namespace Vezeeta_Clone.Core.Features.Reviews.Commands.Handlers
{
    public class ReviewDoctorHandler : ResponseHandler, IRequestHandler<MakeReviewCommand, Response<string>>
    {
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly IReviewService _reviewService;
        public ReviewDoctorHandler(IStringLocalizer<SharedResources> localizer,
                                            ICurrentUserService currentUserService,
                                                     IMapper mapper,
                                                     IReviewService reviewService) : base(localizer)
        {
            _localizer = localizer;
            _currentUserService = currentUserService;
            _mapper = mapper;
            _reviewService = reviewService;
        }

        public async Task<Response<string>> Handle(MakeReviewCommand request, CancellationToken cancellationToken)
        {
            var patientId = _currentUserService.GetCurrentUserId();
            var mappedReview = _mapper.Map<Review>(request);
            try
            {
                await _reviewService.MakeReviewAsync(mappedReview, patientId);
                return Success<string>(null, null, _localizer[SharedResourcesKeys.AddSuccess]);
            }
            catch (InvalidOperationException ex)
            {
                if (ex.Message.Contains("DoesnotHaveAppointment"))
                {
                    return BadRequest<string>(_localizer[SharedResourcesKeys.ReviewWithoutAppointment]);
                }
                else if (ex.Message.Contains("AlreadyReviewed"))
                {
                    return BadRequest<string>(_localizer[SharedResourcesKeys.ReviewAlreadyExists]);
                }
            }
            return BadRequest<string>(_localizer[SharedResourcesKeys.FailToAdd]);
        }
    }
}
