using MediatR;
using Microsoft.Extensions.Localization;
using Vezeeta_Clone.Core.Bases;
using Vezeeta_Clone.Core.Features.Payments.Commands.Models;
using Vezeeta_Clone.Core.Resources;
using Vezeeta_Clone.Data.Entities.Enums;
using Vezeeta_Clone.Service.Abstract;
using Vezeeta_Clone.Service.BackgroundJobServices.Abstract;

namespace Vezeeta_Clone.Core.Features.Payments.Commands.Handlers
{
    public class UpdateAppointmentAfterPaymentCommandHandler : ResponseHandler,
        IRequestHandler<UpdateAppointmentAfterPaymentCommand, Response<string>>
    {
        private readonly IPaymentService _paymentService;
        private readonly IAppointmentService _appointmentService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IBackgroundJobService _backgroundJobService;

        public UpdateAppointmentAfterPaymentCommandHandler(
            IPaymentService paymentService,
            IAppointmentService appointmentService,
            IBackgroundJobService backgroundJobService,
            IStringLocalizer<SharedResources> localizer) : base(localizer)
        {
            _paymentService = paymentService;
            _appointmentService = appointmentService;
            _backgroundJobService = backgroundJobService;
            _localizer = localizer;
        }

        public async Task<Response<string>> Handle(
            UpdateAppointmentAfterPaymentCommand request,
            CancellationToken cancellationToken)
        {
            // ENHANCEMENT 1: Validate request
            if (request?.PaymentId <= 0)
                return BadRequest<string>(_localizer[SharedResourcesKeys.BadRequest]);

            // ENHANCEMENT 2: Get appointment with payment status check
            var appointment = await _paymentService.UpdateAppointmentStatusAfterPaymentAsync(
                request.PaymentId,
                request.IsPaid);

            if (appointment == null)
                return NotFound<string>(_localizer[SharedResourcesKeys.AppointmentBookFailed]);

            // ENHANCEMENT 3: Load full appointment details with null safety
            var appointmentDetails = await _appointmentService
                .GetAppointmentByIdWithIncludesAsync(appointment.ID);

            if (appointmentDetails?.Patient?.ApplicationUser == null)
                return BadRequest<string>(_localizer[SharedResourcesKeys.UserNotFound]);

            // ENHANCEMENT 4: Validate payment object
            if (appointmentDetails.Payment == null)
                return BadRequest<string>(_localizer[SharedResourcesKeys.PaymentFailed]);

            // ENHANCEMENT 5: Extract and prepare email content
            var isPaymentSuccessful = request.IsPaid && 
                appointmentDetails.Payment.Status == PaymentStatus.Paid;

            var emailBody = isPaymentSuccessful
                ? BuildSuccessfulPaymentEmailBody(appointmentDetails)
                : BuildFailedPaymentEmailBody(appointmentDetails);

            var emailSubject = isPaymentSuccessful
                ? _localizer[SharedResourcesKeys.AppointmentConfirmation]
                : _localizer[SharedResourcesKeys.PaymentFailed];

            // ENHANCEMENT 6: Build full HTML email
            var htmlMessage = BuildEmailTemplate(emailBody);

            // ENHANCEMENT 7: Queue background email job with error handling
            try
            {
                await _backgroundJobService.EnqueueAsync<IEmailService>(
                    x => x.SendEmail(
                        appointmentDetails.Patient.ApplicationUser.Email,
                        htmlMessage,
                        emailSubject
                    )
                );
            }
            catch (Exception ex)
            {
                // Log but don't fail the request
                // In production, use ILogger to log this
                System.Diagnostics.Debug.WriteLine($"Failed to enqueue email: {ex.Message}");
            }

            return Success<string>(
                string.Empty,
                message: isPaymentSuccessful
                    ? _localizer[SharedResourcesKeys.PaymentSuccess]
                    : _localizer[SharedResourcesKeys.PaymentFailed]
            );
        }

        /// <summary>
        /// ENHANCEMENT: Build successful payment email body with null safety
        /// </summary>
        private string BuildSuccessfulPaymentEmailBody(Data.Entities.Appointment appointmentDetails)
        {
            var patientName = appointmentDetails.Patient?.ApplicationUser?.FirstName ?? "Patient";
            var doctorName = GetSafeDoctorFullName(appointmentDetails.Doctor);
            var appointmentDate = GetSafeAppointmentDate(appointmentDetails.AvailableSlot);
            var appointmentTime = GetSafeAppointmentTime(appointmentDetails.AvailableSlot);
            var clinicName = appointmentDetails.Doctor?.Clinic?.Name ?? "Clinic";

            return $@"
                <h2>{_localizer[SharedResourcesKeys.AppName]} - {_localizer[SharedResourcesKeys.AppointmentConfirmation]} ✅</h2>

                <p>{_localizer["DearPatient"]}: {patientName},</p>

                <p>{_localizer["AppointmentConfirmedMessage"]}</p>

                <hr/>

                <h3>{_localizer["AppointmentDetails"]}:</h3>
                <ul>
                    <li><strong>{_localizer["Doctor"]}:</strong> Dr. {doctorName}</li>
                    <li><strong>{_localizer["Date"]}:</strong> {appointmentDate}</li>
                    <li><strong>{_localizer["Time"]}:</strong> {appointmentTime}</li>
                    <li><strong>{_localizer["Clinic"]}:</strong> {clinicName}</li>
                </ul>
                <hr/>

                <p>{_localizer["ThankYouMessage"]}</p>
            ";
        }

        /// <summary>
        /// ENHANCEMENT: Build failed payment email body with localization
        /// </summary>
        private string BuildFailedPaymentEmailBody(Data.Entities.Appointment appointmentDetails)
        {
            var patientName = appointmentDetails.Patient?.ApplicationUser?.FirstName ?? "Patient";
            var doctorName = GetSafeDoctorFullName(appointmentDetails.Doctor);
            var appointmentDate = GetSafeAppointmentDate(appointmentDetails.AvailableSlot);
            var appointmentTime = GetSafeAppointmentTime(appointmentDetails.AvailableSlot);
            var clinicName = appointmentDetails.Doctor?.Clinic?.Name ?? "Clinic";
            var failureReason = appointmentDetails.Payment?.FailureReason ?? "Unknown";

            return $@"
                <h2>{_localizer[SharedResourcesKeys.AppName]} - {_localizer[SharedResourcesKeys.PaymentFailed]} ❌</h2>

                <p>{_localizer["DearPatient"]}: {patientName},</p>

                <p>{_localizer["PaymentFailedMessage"]}</p>

                <hr/>

                <h3>{_localizer["AppointmentDetails"]}:</h3>
                <ul>
                    <li><strong>{_localizer["Doctor"]}:</strong> Dr. {doctorName}</li>
                    <li><strong>{_localizer["Date"]}:</strong> {appointmentDate}</li>
                    <li><strong>{_localizer["Time"]}:</strong> {appointmentTime}</li>
                    <li><strong>{_localizer["Clinic"]}:</strong> {clinicName}</li>
                </ul>

                <hr/>

                <p><strong>{_localizer["FailureReason"]}:</strong> {failureReason}</p>

                <p>{_localizer["RetryMessage"]}</p>

                <hr/>
            ";
        }

        /// <summary>
        /// ENHANCEMENT: Build complete HTML email template with proper styling
        /// </summary>
        private string BuildEmailTemplate(string bodyContent)
        {
            return $@"
                <table style='width:100%; font-family:Arial, sans-serif; background-color:#f4f4f4; padding:40px 0;'>
                  <tr>
                    <td align='center'>
                      <!-- Main Container -->
                      <table style='width:600px; background-color:#ffffff; border-radius:12px; box-shadow:0 4px 20px rgba(0,0,0,0.1); overflow:hidden; text-align:left;'>
        
                        <!-- Logo Section -->
                        <tr>
                          <td style='padding:30px 0; text-align:center;'>
                            <img src='https://res.cloudinary.com/ddtcswz77/image/upload/v1774491046/Logo_lotycd.png' alt='Logo' width='100' style='display:block; margin:0 auto;'/>
                          </td>
                        </tr>

                        <!-- App Name -->
                        <tr>
                          <td style='padding-bottom:20px; text-align:center;'>
                            <h2 style='margin:0; font-size:24px; color:#333333;'>{_localizer[SharedResourcesKeys.AppName]}</h2>
                          </td>
                        </tr>

                        <!-- Body Section -->
                        <tr>
                          <td style='padding:0 30px 30px 30px; color:#333333; font-size:16px; line-height:1.6;'>
                            {bodyContent}
                          </td>
                        </tr>

                        <!-- Footer Section -->
                        <tr>
                          <td style='padding:20px; text-align:center; font-size:12px; color:#888888; background-color:#f9f9f9;'>
                            {_localizer["DisclaimerMessage"]}
                          </td>
                        </tr>

                      </table>
                    </td>
                  </tr>
                </table>
            ";
        }

        /// <summary>
        /// ENHANCEMENT: Safely extract doctor's full name with null coalescing
        /// </summary>
        private string GetSafeDoctorFullName(Data.Entities.Doctor doctor)
        {
            if (doctor?.ApplicationUser == null)
                return _localizer["UnknownDoctor"] ?? "Doctor";

            var firstName = doctor.ApplicationUser.FirstName ?? string.Empty;
            var lastName = doctor.ApplicationUser.LastName ?? string.Empty;

            return $"{firstName} {lastName}".Trim();
        }

        /// <summary>
        /// ENHANCEMENT: Safely extract appointment date
        /// </summary>
        private string GetSafeAppointmentDate(Data.Entities.DoctorAvailabilitySlot slot)
        {
            if (slot?.Date == null)
                return _localizer["DateNotAvailable"] ?? "N/A";

            try
            {
                return slot.Date.Value.ToString("dddd, MMMM dd, yyyy");
            }
            catch
            {
                return _localizer["DateFormatError"] ?? "N/A";
            }
        }

        /// <summary>
        /// ENHANCEMENT: Safely extract appointment time
        /// </summary>
        private string GetSafeAppointmentTime(Data.Entities.DoctorAvailabilitySlot slot)
        {
            if (slot?.StartTime == null)
                return _localizer["TimeNotAvailable"] ?? "N/A";

            try
            {
                return slot.StartTime.ToString();
            }
            catch
            {
                return _localizer["TimeFormatError"] ?? "N/A";
            }
        }
    }
}