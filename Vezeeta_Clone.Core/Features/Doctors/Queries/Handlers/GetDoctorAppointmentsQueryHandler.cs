using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System.Linq.Expressions;
using Vezeeta_Clone.Core.Bases;
using Vezeeta_Clone.Core.Features.Doctors.Queries.Models;
using Vezeeta_Clone.Core.Features.Doctors.Queries.Results;
using Vezeeta_Clone.Core.Resources;
using Vezeeta_Clone.Core.Wrappers;
using Vezeeta_Clone.Data.Commons;
using Vezeeta_Clone.Data.Entities;
using Vezeeta_Clone.Service.Abstract;
using Vezeeta_Clone.Service.AppUserAuthServices.Abstract;

namespace Vezeeta_Clone.Core.Features.Doctors.Queries.Handlers
{
    public class GetDoctorAppointmentsQueryHandler : ResponseHandler, IRequestHandler<GetDoctorAppointmentsQuery, Response<PaginatedResult<GetDoctorAppointmentsQueryResult>>>
    {
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly ICurrentUserService _currentUserService;
        private readonly IAppointmentService _appointmentService;
        private readonly IMapper _mapper;
        public GetDoctorAppointmentsQueryHandler(IStringLocalizer<SharedResources> localizer,
                                               ICurrentUserService currentUserService,
                                                  IAppointmentService appointmentService,
                                               IMapper mapper) : base(localizer)
        {
            _localizer = localizer;
            _currentUserService = currentUserService;
            _appointmentService = appointmentService;
            _mapper = mapper;
        }

        public async Task<Response<PaginatedResult<GetDoctorAppointmentsQueryResult>>> Handle(GetDoctorAppointmentsQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Appointment, GetDoctorAppointmentsQueryResult>> expression =
               ex => new GetDoctorAppointmentsQueryResult(ex.ID,
               string.Concat(ex.ActualPatientName ?? ex.Patient.ApplicationUser.FirstName, "", ex.Patient.ApplicationUser.LastName),
               ex.ActualPatientPhone ?? ex.Patient.ApplicationUser.PhoneNumber,
               ex.AvailableSlot.Date, ex.AvailableSlot.StartTime, ex.AvailableSlot.EndTime,

               ex.Status.ToString());


            var roles = await _currentUserService.GetCurrentUserRolesAsync();
            var doctorId = string.Empty;
            if (roles.Contains(Roles.Doctor))
            {
                doctorId = _currentUserService.GetCurrentUserId();
            }
            else
            {
                return Unauthorized<PaginatedResult<GetDoctorAppointmentsQueryResult>>(_localizer[SharedResourcesKeys.UnAuthorized]);
            }

            var filteredAppointments = _appointmentService.GetDoctorAppointmentsAsync(doctorId, request.status, request.availabilityMethod, request.FromDate, request.ToDate);
            var paginatedResult = await filteredAppointments.Select(expression).ToPaginatedListAsync(request.PageNumber, request.PageSize);
            if (paginatedResult.TotalCount == 0)
                return NotFound<PaginatedResult<GetDoctorAppointmentsQueryResult>>(_localizer[SharedResourcesKeys.NoData]);
            return Success(paginatedResult);
        }
    }
}
