using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Vezeeta_Clone.Core.Bases;
using Vezeeta_Clone.Core.Features.Appointments.Commands.Models;
using Vezeeta_Clone.Core.Resources;
using Vezeeta_Clone.Data.Commons;
using Vezeeta_Clone.Data.Entities;
using Vezeeta_Clone.Service.Abstract;
using Vezeeta_Clone.Service.AppUserAuthServices.Abstract;

namespace Vezeeta_Clone.Core.Features.Appointments.Commands.Handlers
{
    public class BookAppointmentCommandHandler : ResponseHandler, IRequestHandler<BookAppointmentCommand, Response<string>>
    {
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly IAppointmentService _appointmentService;
        public BookAppointmentCommandHandler(IStringLocalizer<SharedResources> localizer,
                                        IAppointmentService appointmentService,
                                        ICurrentUserService currentUserService,
                                        IMapper mapper) : base(localizer)
        {
            _localizer = localizer;
            _mapper = mapper;
            _appointmentService = appointmentService;
            _currentUserService = currentUserService;
        }

        public async Task<Response<string>> Handle(BookAppointmentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var roles = await _currentUserService.GetCurrentUserRolesAsync();
                string patientId = string.Empty;

                if (roles.Contains(Roles.Patient))
                {
                    patientId = _currentUserService.GetCurrentUserId();
                }
                else
                {
                    return Unauthorized<string>();
                }

                var mappedBooking = _mapper.Map<Appointment>(request);
                var appointmentId = await _appointmentService.BookAppointmentAsync(mappedBooking, patientId);

                return Success<string>(appointmentId.ToString(), message: _localizer[SharedResourcesKeys.AppointmentBooked]);
            }
            catch (InvalidOperationException ex)
            {
                if (ex.Message.Contains("notfound"))
                {
                    return BadRequest<string>(_localizer[SharedResourcesKeys.NotFound]);
                }
                else if (ex.Message.Contains("Slotislocked"))
                {
                    return BadRequest<string>(_localizer[SharedResourcesKeys.SlotIsLocked]);
                }
                else if (ex.Message.Contains("pastslots"))
                {
                    return BadRequest<string>(_localizer[SharedResourcesKeys.PastSlot]);
                }
                else if (ex.Message.Contains("alreadybooked"))
                {
                    return BadRequest<string>(_localizer[SharedResourcesKeys.SlotAlreadyBooked]);
                }
                return BadRequest<string>(ex.Message);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return BadRequest<string>(_localizer[SharedResourcesKeys.AppointmentBookFailed]);
            }
            catch (Exception ex)
            {
                return BadRequest<string>(ex.Message);
            }

        }
    }
}
