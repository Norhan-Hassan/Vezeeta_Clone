using MediatR;
using Microsoft.Extensions.Localization;
using System.Linq.Expressions;
using Vezeeta_Clone.Core.Bases;
using Vezeeta_Clone.Core.Features.Patients.Queries.Models;
using Vezeeta_Clone.Core.Features.Patients.Queries.Results;
using Vezeeta_Clone.Core.Resources;
using Vezeeta_Clone.Core.Wrappers;
using Vezeeta_Clone.Data.Commons;
using Vezeeta_Clone.Data.Entities;
using Vezeeta_Clone.Service.Abstract;
using Vezeeta_Clone.Service.AppUserAuthServices.Abstract;

namespace Vezeeta_Clone.Core.Features.Patients.Queries.Handlers
{
    public class PatientAppointmentHandler : ResponseHandler, IRequestHandler<GetPatientAppointmentsQuery, Response<PaginatedResult<GetPatientAppointmentsQueryResult>>>
    {
        private readonly IMediator _mediator;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly ICurrentUserService _currentUserService;
        private readonly IAppointmentService _appointmentService;

        public PatientAppointmentHandler(IStringLocalizer<SharedResources> localizer,
                                            IMediator mediator,
                                            IAppointmentService appointmentService,
                                            ICurrentUserService currentUserService) : base(localizer)
        {
            _localizer = localizer;
            _mediator = mediator;
            _appointmentService = appointmentService;
            _currentUserService = currentUserService;
        }

        public async Task<Response<PaginatedResult<GetPatientAppointmentsQueryResult>>> Handle(GetPatientAppointmentsQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Appointment, GetPatientAppointmentsQueryResult>> expression =
               ex => new GetPatientAppointmentsQueryResult(ex.ID,
              ex.ActualPatientName ?? ex.Patient.ApplicationUser.FirstName + " " + ex.Patient.ApplicationUser.LastName,
               ex.ActualPatientPhone ?? ex.Patient.ApplicationUser.PhoneNumber,
               ex.Doctor.ApplicationUser.FirstName + " " + ex.Doctor.ApplicationUser.LastName,
               ex.AvailableSlot.Date, ex.AvailableSlot.StartTime, ex.Status.ToString());

            var roles = await _currentUserService.GetCurrentUserRolesAsync();
            var patientId = string.Empty;
            if (roles.Contains(Roles.Patient))
            {
                patientId = _currentUserService.GetCurrentUserId();
            }
            else
            {
                return Unauthorized<PaginatedResult<GetPatientAppointmentsQueryResult>>(_localizer[SharedResourcesKeys.UnAuthorized]);
            }

            var filteredAppointments = _appointmentService.GetPatientAppointmentsAsync(patientId, request.Status);
            var paginatedResult = await filteredAppointments.Select(expression).ToPaginatedListAsync(request.PageNumber, request.PageSize);
            if (paginatedResult.TotalCount == 0)
                return NotFound<PaginatedResult<GetPatientAppointmentsQueryResult>>(_localizer[SharedResourcesKeys.NoData]);
            return Success(paginatedResult);
        }
    }
}
