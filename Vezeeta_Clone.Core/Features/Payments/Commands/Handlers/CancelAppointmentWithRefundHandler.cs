using MediatR;
using Microsoft.Extensions.Localization;
using Vezeeta_Clone.Core.Bases;
using Vezeeta_Clone.Core.Features.Payments.Commands.Models;
using Vezeeta_Clone.Core.Resources;
using Vezeeta_Clone.Service.Abstract;
using Vezeeta_Clone.Service.BackgroundJobServices.Abstract;

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

            var body = $@"
                        <h2>Appointment Cancelled </h2>

                        <p>Dear {patientName},</p>

                        <p>Your appointment has been successfully cancelled.</p>

                        <hr/>

                        <h3>Appointment Details:</h3>
                        <ul>
                            <li><strong>Doctor:</strong> Dr. {doctorName}</li>
                            <li><strong>Date:</strong> {appointmentDetails.AvailableSlot?.Date:dddd, MMMM dd, yyyy}</li>
                            <li><strong>Time:</strong> {appointmentDetails.AvailableSlot?.StartTime}</li>
                            <li><strong>Clinic:</strong> {appointmentDetails.Doctor?.Clinic?.Name}</li>
                        </ul>

                        <p><strong>Reason:</strong> {request.CancellationReason}</p>

                        <hr/>

                        <p>If a payment was made, your refund will be processed shortly.</p>
                    ";

            var message = $@"
                    <table style='width:100%; font-family:Arial; background-color:#f4f4f4; padding:40px 0;'>
                      <tr>
                        <td align='center'>
                          <table style='width:600px; background-color:#fff; border-radius:12px; box-shadow:0 4px 20px rgba(0,0,0,0.1); text-align:center;'>

                            <tr>
                              <td style='padding:30px 0;'>
                                <img src='https://res.cloudinary.com/ddtcswz77/image/upload/v1774491046/Logo_lotycd.png' width='100'/>
                              </td>
                            </tr>

                            <tr>
                              <td>
                                <h2>{_localizer[SharedResourcesKeys.AppName]}</h2>
                              </td>
                            </tr>

                            <tr>
                              <td style='padding:20px;'>
                                {body}
                              </td>
                            </tr>

                            <tr>
                              <td style='font-size:12px; color:#888; padding:20px;'>
                                If you did not request this, ignore this email.
                              </td>
                            </tr>

                          </table>
                        </td>
                      </tr>
                    </table>";


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
