using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using Vezeeta_Clone.Core.Bases;
using Vezeeta_Clone.Core.Features.Appointments.Queries.Models;
using Vezeeta_Clone.Core.Features.Appointments.Queries.Results;
using Vezeeta_Clone.Core.Resources;
using Vezeeta_Clone.Service.Abstract;

namespace Vezeeta_Clone.Core.Features.Appointments.Queries.Handlers
{
    public class AppointmentQueryHandler : ResponseHandler, IRequestHandler<GetAppointmentDetailesQuery, Response<GetAppointmentDetailsQueryResult>>
    {
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IMapper _mapper;
        private readonly IAppointmentService _appointmentService;
        public AppointmentQueryHandler(IStringLocalizer<SharedResources> localizer,
                                        IAppointmentService appointmentService,
                                        IMapper mapper) : base(localizer)
        {
            _appointmentService = appointmentService;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Response<GetAppointmentDetailsQueryResult>> Handle(GetAppointmentDetailesQuery request, CancellationToken cancellationToken)
        {

            var appointment = await _appointmentService.GetAppointmentByIdAsync(request.Id);
            if (appointment == null)
            {
                return NotFound<GetAppointmentDetailsQueryResult>(_localizer[SharedResourcesKeys.NotFound]);
            }

            var mappedAppointment = _mapper.Map<GetAppointmentDetailsQueryResult>(appointment);

            return Success<GetAppointmentDetailsQueryResult>(mappedAppointment);
            throw new NotImplementedException();
        }
    }
}
