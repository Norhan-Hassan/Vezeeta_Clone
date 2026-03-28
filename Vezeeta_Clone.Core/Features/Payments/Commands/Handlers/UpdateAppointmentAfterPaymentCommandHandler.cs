using MediatR;
using Microsoft.Extensions.Localization;
using Vezeeta_Clone.Core.Bases;
using Vezeeta_Clone.Core.Features.Payments.Commands.Models;
using Vezeeta_Clone.Core.Resources;
using Vezeeta_Clone.Data.Entities.Enums;
using Vezeeta_Clone.Service.Abstract;
using Vezeeta_Clone.Service.BackgroundJobServices.Abstract;
using Vezeeta_Clone.Service.ExternalServices.Abstract;

namespace Vezeeta_Clone.Core.Features.Payments.Commands.Handlers
{
    public class UpdateAppointmentAfterPaymentCommandHandler : ResponseHandler,
      IRequestHandler<UpdateAppointmentAfterPaymentCommand, Response<string>>
    {
        private readonly IPaymentService _paymentService;
        private readonly IAppointmentService _appointmentService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IEmailService _emailService;
        private readonly IBackgroundJobService _backgroundJobService;


        public UpdateAppointmentAfterPaymentCommandHandler(IPaymentService paymentService,
            IAppointmentService appointmentService,
            IBackgroundJobService backgroundJobService,
            IEmailService emailService,
            IStringLocalizer<SharedResources> localizer) : base(localizer)
        {
            _paymentService = paymentService;
            _appointmentService = appointmentService;
            _emailService = emailService;
            _backgroundJobService = backgroundJobService;
            _localizer = localizer;
        }

        public async Task<Response<string>> Handle(UpdateAppointmentAfterPaymentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var appointment = await _paymentService.UpdateAppointmentStatusAfterPaymentAsync(request.PaymentId, request.IsPaid);
                if (appointment == null)
                    return NotFound<string>();

                var appointmentDetails = await _appointmentService.GetAppointmentByIdWithIncludesAsync(appointment.ID);

                if (appointmentDetails.Status == AppointmentStatus.Confirmed && appointmentDetails.Payment.Status == PaymentStatus.Paid)
                {
                    try
                    {
                        var variables = new Dictionary<string, string>
                        {
                            { "BodyText", _localizer[SharedResourcesKeys.ConfirmEmailBody] },
                            { "EmailFooter", _localizer[SharedResourcesKeys.EmailFooter] },
                            { "AppointmentConfirmation", _localizer[SharedResourcesKeys.AppointmentConfirmation] },
                            { "BookingConfirmation", _localizer[SharedResourcesKeys.BookingConfirmation] },
                            { "AppointmentDetails", _localizer[SharedResourcesKeys.AppointmentDetails] },
                            { "Dear", _localizer[SharedResourcesKeys.Dear] },
                            { "Doctor", _localizer[SharedResourcesKeys.Doctor] },
                            { "Date", _localizer[SharedResourcesKeys.Date] },
                            { "Time", _localizer[SharedResourcesKeys.Time] },
                            { "Clinic", _localizer[SharedResourcesKeys.Clinic] },
                            { "ThanksMessage", _localizer[SharedResourcesKeys.ThanksMessage] },
                        };

                        var template = await _emailService.LoadEmailTemplateAsync("AppointmentBookingConfirmation.html", variables);

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


                        await _backgroundJobService.EnqueueAsync<IEmailService>(
                            x => x.SendEmail(
                                appointmentDetails.Patient.ApplicationUser.Email,
                                message,
                                _localizer[SharedResourcesKeys.AppointmentConfirmation]
                            )
                        );
                        return Success<string>("");
                    }
                    catch (FileNotFoundException fileEx)
                    {
                        return BadRequest<string>($"Email template not found: {fileEx.Message}");
                    }
                }
                return BadRequest<string>(_localizer[SharedResourcesKeys.PaymentFailed]);
            }
            catch (InvalidOperationException ex)
            {
                if (ex.Message.Contains("PendingAppointment"))
                {
                    return NotFound<string>(_localizer[SharedResourcesKeys.CompleteAppointment]);
                }
                if (ex.Message.Contains("alreadyconfirmedandpaid"))
                {
                    return BadRequest<string>(_localizer[SharedResourcesKeys.AlreadyPaid]);
                }
                else
                {
                    return BadRequest<string>(ex.Message);
                }
            }
            catch (Exception ex)
            {
                return BadRequest<string>(ex.Message);
            }
        }
    }
}

