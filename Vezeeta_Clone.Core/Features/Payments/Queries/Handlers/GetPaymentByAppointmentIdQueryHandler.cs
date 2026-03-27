using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using Vezeeta_Clone.Core.Bases;
using Vezeeta_Clone.Core.Features.Payments.Queries.Models;
using Vezeeta_Clone.Core.Features.Payments.Queries.Results;
using Vezeeta_Clone.Core.Resources;
using Vezeeta_Clone.Service.Abstract;

namespace Vezeeta_Clone.Core.Features.Payments.Queries.Handlers
{
    public class GetPaymentByAppointmentIdQueryHandler : ResponseHandler, IRequestHandler<GetPaymentByAppointmentIdQuery, Response<GetPaymentInfoQueryResult>>
    {
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IAppointmentService _appointmentService;

        public GetPaymentByAppointmentIdQueryHandler(IAppointmentService appointmentService, IMapper mapper,
            IStringLocalizer<SharedResources> localizer) : base(localizer)
        {

            _mapper = mapper;
            _localizer = localizer;
            _appointmentService = appointmentService;
        }

        public async Task<Response<GetPaymentInfoQueryResult>> Handle(GetPaymentByAppointmentIdQuery request, CancellationToken cancellationToken)
        {

            var appointment = await _appointmentService.GetAppointmentByIdAsync(request.AppointmentId);
            if (appointment?.Payment == null)
                return NotFound<GetPaymentInfoQueryResult>();

            return Success(_mapper.Map<GetPaymentInfoQueryResult>(appointment.Payment));
        }
    }
}
