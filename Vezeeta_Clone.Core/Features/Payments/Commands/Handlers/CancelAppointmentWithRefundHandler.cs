using MediatR;
using Microsoft.Extensions.Localization;
using Vezeeta_Clone.Core.Bases;
using Vezeeta_Clone.Core.Features.Payments.Commands.Models;
using Vezeeta_Clone.Core.Resources;
using Vezeeta_Clone.Service.Abstract;
using Vezeeta_Clone.Service.BackgroundJobServices.Abstract;
using Vezeeta_Clone.Service.ExternalServices.Abstract;

namespace Vezeeta_Clone.Core.Features.Payments.Commands.Handlers
{
    public class CancelAppointmentWithRefundHandler : ResponseHandler, IRequestHandler<CancelAppointmentWithRefundCommand, Response<string>>
    {
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IPaymentService _paymentService;
        private readonly IEmailService _emailService;
        private readonly IBackgroundJobService _backgroundJobService;
        private readonly IAppointmentService _appointmentService;

        public CancelAppointmentWithRefundHandler(IEmailService emailService,
            IPaymentService paymentService,
            IBackgroundJobService backgroundJobService,
                IAppointmentService appointmentService,
            IStringLocalizer<SharedResources> localizer) : base(localizer)
        {
            _paymentService = paymentService;
            _localizer = localizer;
            _appointmentService = appointmentService;
            _backgroundJobService = backgroundJobService;
            _emailService = emailService;
        }

        public async Task<Response<string>> Handle(CancelAppointmentWithRefundCommand request, CancellationToken cancellationToken)
        {
            var isCancelled = await _paymentService.CancelAppointmentWithRefundAsync(
                request.AppointmentId,
                request.CancellationReason
            );

            if (!isCancelled)
                return BadRequest<string>(_localizer[SharedResourcesKeys.FailedToCancellAppointment]);


            var appointmentDetails = await _appointmentService.GetAppointmentByIdWithIncludesAsync(request.AppointmentId);

            if (appointmentDetails == null)
                return NotFound<string>();

            var patientName = appointmentDetails.Patient?.ApplicationUser?.FirstName ?? "User";
            var doctorName = appointmentDetails.Doctor?.ApplicationUser != null
                ? $"{appointmentDetails.Doctor.ApplicationUser.FirstName} {appointmentDetails.Doctor.ApplicationUser.LastName}"
                : "Doctor";

            var email = appointmentDetails.Patient?.ApplicationUser?.Email;

            if (string.IsNullOrEmpty(email))
                return BadRequest<string>(_localizer[SharedResourcesKeys.EmailIsNotExist]);



            var variables = new Dictionary<string, string>
                        {
                            { "BodyText", _localizer[SharedResourcesKeys.ConfirmEmailBody] },
                            { "EmailFooter", _localizer[SharedResourcesKeys.EmailFooter] },
                            { "AppointmentCancelled", _localizer[SharedResourcesKeys.AppointmentCancelled] },
                            { "CancellationMessage", _localizer[SharedResourcesKeys.CancellationMessage] },
                            { "AppointmentDetails", _localizer[SharedResourcesKeys.AppointmentDetails] },
                            { "Dear", _localizer[SharedResourcesKeys.Dear] },
                            { "Doctor", _localizer[SharedResourcesKeys.Doctor] },
                            { "Date", _localizer[SharedResourcesKeys.Date] },
                            { "Time", _localizer[SharedResourcesKeys.Time] },
                            { "Clinic", _localizer[SharedResourcesKeys.Clinic] },
                            { "ThanksMessage", _localizer[SharedResourcesKeys.ThanksMessage] },
                        };
            var template = await _emailService.LoadEmailTemplateAsync("AppointmentBookingCancellation.html", variables);

            if (string.IsNullOrEmpty(template))
            {
                return BadRequest<string>("Email template is empty");
            }

            var message = template
                .Replace("{PatientFirstName}", appointmentDetails.Patient?.ApplicationUser?.FirstName ?? "")
                .Replace("{DoctorFullName}", $"{appointmentDetails.Doctor?.ApplicationUser?.FirstName ?? ""} {appointmentDetails.Doctor?.ApplicationUser?.LastName ?? ""}".Trim())
                .Replace("{AppointmentDate}", appointmentDetails.AvailableSlot?.Date.ToString("dddd, MMMM dd, yyyy") ?? "")
                .Replace("{AppointmentTime}", appointmentDetails.AvailableSlot?.StartTime.ToShortTimeString() ?? "")
                .Replace("{ClinicName}", appointmentDetails.Doctor?.Clinic?.Name ?? "");


            await _backgroundJobService.EnqueueAsync<IEmailService>(x =>
                x.SendEmail(
                    email,
                    message,
                    _localizer[SharedResourcesKeys.AppointmentCancellation]
                )
            );

            return Success<string>(null, message: _localizer[SharedResourcesKeys.AppointmentCancelled]);
        }
    }
}
