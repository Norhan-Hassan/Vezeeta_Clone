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

        public async Task<Response<string>> Handle(UpdateAppointmentAfterPaymentCommand request,
            CancellationToken cancellationToken)
        {
            var appointment = await _paymentService.UpdateAppointmentStatusAfterPaymentAsync(request.PaymentId, request.IsPaid);
            if (appointment == null)
                return NotFound<string>();

            var appointmentDetails = await _appointmentService.GetAppointmentByIdWithIncludesAsync(appointment.ID);
            var body = string.Empty;
            if (appointment.Status == AppointmentStatus.Confirmed && appointment.Payment.Status == PaymentStatus.Completed)
            {
                body = $@"
                        <h2>Appointment Confirmed </h2>

                        <p>Dear {appointmentDetails.Patient.ApplicationUser.FirstName},</p>

                        <p>Your appointment has been successfully confirmed.</p>

                        <hr/>

                        <h3>Appointment Details:</h3>
                        <ul>
                            <li><strong>Doctor:</strong> Dr. {string.Concat(appointmentDetails.Doctor?.ApplicationUser?.FirstName, ' ', appointmentDetails?.Doctor?.ApplicationUser?.LastName)}</li>
                            <li><strong>Date:</strong> {appointmentDetails.AvailableSlot?.Date.ToString("dddd, MMMM dd, yyyy")}</li>
                            <li><strong>Time:</strong> {appointmentDetails.AvailableSlot?.StartTime}</li>
                            <li><strong>Clinic:</strong> {appointmentDetails.Doctor?.Clinic?.Name}</li>
                        </ul>
                        <hr/>

                        <p>Thank you for choosing our service </p>
                        ";
                var message = $@"
                    <table style='width:100%; font-family:Arial, sans-serif; background-color:#f4f4f4; padding:40px 0;'>
                      <tr>
                        <td align='center'>
                          <!-- Main Container -->
                          <table style='width:600px; background-color:#ffffff; border-radius:12px; box-shadow:0 4px 20px rgba(0,0,0,0.1); overflow:hidden; text-align:center;'>
        
                            <!-- Logo Section -->
                            <tr>
                              <td style='padding:30px 0;'>
                                <!-- Logo -->
                                <img src='https://res.cloudinary.com/ddtcswz77/image/upload/v1774491046/Logo_lotycd.png' alt='Logo' width='100' style='display:block; margin:0 auto;'/>
                              </td>
                            </tr>

                            <!-- App Name -->
                            <tr>
                              <td style='padding-bottom:20px;'>
                                <h2 style='margin:0; font-size:24px; color:#333333;'>{_localizer[SharedResourcesKeys.AppName]}</h2>
                              </td>
                            </tr>

                            <!-- Body Section -->
                            <tr>
                              <td style='padding:0 30px 30px 30px; color:#333333; font-size:16px; line-height:1.6;'>
                                <p style='font-size:18px; font-weight:500; margin-bottom:20px;'>{body}</p>                                          
                              </td>
                            </tr>

                            <!-- Footer Section -->
                            <tr>
                              <td style='padding:20px; text-align:center; font-size:12px; color:#888888; background-color:#f9f9f9;'>
                                If you did not request this email, you can safely ignore it.
                              </td>
                            </tr>

                          </table>
                        </td>
                      </tr>
                    </table>";
                await _backgroundJobService.EnqueueAsync<IEmailService>(
                           x => x.SendEmail(
                               appointmentDetails.Patient.ApplicationUser.Email,
                               message,
                               _localizer[SharedResourcesKeys.AppointmentConfirmation]
                           )
                       );
                return Success<string>("");

            }

            return BadRequest<string>(_localizer[SharedResourcesKeys.PaymentFailed]);
        }
    }
}
