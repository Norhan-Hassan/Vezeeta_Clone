using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System.Linq.Expressions;
using Vezeeta_Clone.Core.Bases;
using Vezeeta_Clone.Core.Features.Doctors.Queries.Models;
using Vezeeta_Clone.Core.Features.Doctors.Queries.Results;
using Vezeeta_Clone.Core.Resources;
using Vezeeta_Clone.Core.Wrappers;
using Vezeeta_Clone.Data.Entities;
using Vezeeta_Clone.Service.Abstract;

namespace Vezeeta_Clone.Core.Features.Doctors.Queries.Handlers
{
    public class DoctorsQueryHandler : ResponseHandler,
        IRequestHandler<GetDoctorsPaginatedQuery, Response<PaginatedResult<GetDoctorsPaginatedQueryResult>>>,
        IRequestHandler<GetDoctorReviewsQuery, Response<PaginatedResult<GetDoctorReviewsQueryResult>>>,
        IRequestHandler<GetDoctorDetailsQuery, Response<GetDoctorDetailsQueryResult>>,
        IRequestHandler<GetDoctorExaminationDetailsQuery, Response<GetDoctorExaminationDetailsQueryResult>>


    {
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IDoctorService _doctorService;
        private readonly IMapper _mapper;
        public DoctorsQueryHandler(IStringLocalizer<SharedResources> localizer,
                                               IDoctorService doctorService,
                                               IMapper mapper) : base(localizer)
        {
            _localizer = localizer;
            _doctorService = doctorService;
            _mapper = mapper;
        }

        public async Task<Response<PaginatedResult<GetDoctorsPaginatedQueryResult>>> Handle(GetDoctorsPaginatedQuery request, CancellationToken cancellationToken)
        {

            Expression<Func<Doctor, GetDoctorsPaginatedQueryResult>> expression =
                ex => new GetDoctorsPaginatedQueryResult(
                    string.Concat(ex.ApplicationUser.FirstName, " ", ex.ApplicationUser.LastName),
                    ex.ExperienceInYears,
                    ex.Specialization!.LocalizedName,
                    ex.Clinic!.Region.LocalizedName,
                    ex.Picture);

            var filteredQuery = _doctorService.FilteredDoctorsAsQuerable(request.specializationId, request.Search, request.cityId, request.regionId, request.OrderBy);

            var paginatedResult = await filteredQuery.Select(expression).ToPaginatedListAsync(request.PageNumber, request.PageSize);
            if (paginatedResult.TotalCount == 0)
                return NotFound<PaginatedResult<GetDoctorsPaginatedQueryResult>>(_localizer[SharedResourcesKeys.NoData]);
            return Success(paginatedResult);
        }



        public async Task<Response<PaginatedResult<GetDoctorReviewsQueryResult>>> Handle(GetDoctorReviewsQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Review, GetDoctorReviewsQueryResult>> expression =
                ex => new GetDoctorReviewsQueryResult(ex.Rating, ex.Comment, ex.CreatedAt, string.Concat(ex.Patient!.ApplicationUser.FirstName, " ", ex.Patient.ApplicationUser.LastName), ex.Patient.GetAge());
            var reviewsQuery = _doctorService.GetDoctorReviews(request.DoctorId);
            var paginatedResult = await reviewsQuery.Select(expression).ToPaginatedListAsync(request.PageNumber, request.PageSize);
            if (paginatedResult.TotalCount == 0)
                return NotFound<PaginatedResult<GetDoctorReviewsQueryResult>>(_localizer[SharedResourcesKeys.NoData]);
            return Success(paginatedResult);
        }
        public async Task<Response<GetDoctorDetailsQueryResult>> Handle(GetDoctorDetailsQuery request, CancellationToken cancellationToken)
        {
            var doctor = await _doctorService.GetDoctorByIDAsync(request.Id);
            if (doctor == null)
                return NotFound<GetDoctorDetailsQueryResult>(_localizer[SharedResourcesKeys.NotFound]);
            //mapping
            var doctorMapped = _mapper.Map<GetDoctorDetailsQueryResult>(doctor);
            var (average, total) = await _doctorService.GetDoctorRatingInfo(doctor.AppUserID);
            doctorMapped.AverageRating = average;
            doctorMapped.TotalRating = total;
            return Success(doctorMapped);
        }



        public async Task<Response<GetDoctorExaminationDetailsQueryResult>> Handle(GetDoctorExaminationDetailsQuery request, CancellationToken cancellationToken)
        {
            var doctor = await _doctorService.GetDoctorWithClinicByIDAsync(request.Id);
            if (doctor == null)
                return NotFound<GetDoctorExaminationDetailsQueryResult>(_localizer[SharedResourcesKeys.NotFound]);

            var doctorExaminationDetailsMapped = _mapper.Map<GetDoctorExaminationDetailsQueryResult>(doctor);
            return Success(doctorExaminationDetailsMapped);
        }




    }
}
