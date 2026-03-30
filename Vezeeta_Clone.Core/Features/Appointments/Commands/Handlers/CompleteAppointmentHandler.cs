using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using Vezeeta_Clone.Core.Bases;
using Vezeeta_Clone.Core.Features.Appointments.Commands.Models;
using Vezeeta_Clone.Core.Resources;
using Vezeeta_Clone.Service.Abstract;

namespace Vezeeta_Clone.Core.Features.Appointments.Commands.Handlers
{
    public class CompleteAppointmentHandler : ResponseHandler,
        IRequestHandler<CompleteAppointmentCommand, Response<string>>
    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IMapper _mapper;
        private readonly IAppointmentService _appointmentService;
        #endregion

        #region Constructor
        public CompleteAppointmentHandler(IStringLocalizer<SharedResources> localizer,
                                        IAppointmentService appointmentService,
                                        IMapper mapper) : base(localizer)
        {
            _appointmentService = appointmentService;
            _mapper = mapper;
            _localizer = localizer;
        }
        #endregion

        #region Functions

        public async Task<Response<string>> Handle(CompleteAppointmentCommand request, CancellationToken cancellationToken)
        {
            var appointment = await _appointmentService.GetAppointmentByIdAsync(request.AppointmentId);
            if (appointment == null)
            {
                return NotFound<string>();
            }
            var mappedAppointment = _mapper.Map(request, appointment);
            var result = await _appointmentService.CompleteAppointmentAsync(mappedAppointment);
            if (result)
            {
                return Success<string>("", message: _localizer[SharedResourcesKeys.BookingCompleted]);
            }
            return BadRequest<string>(_localizer[SharedResourcesKeys.BookingCompletionFailed]);
        }

        #endregion
    }
}
