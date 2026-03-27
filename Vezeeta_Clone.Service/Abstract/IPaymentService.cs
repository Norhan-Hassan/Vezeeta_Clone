using Vezeeta_Clone.Data.Entities;
using Vezeeta_Clone.Data.Entities.Enums;

namespace Vezeeta_Clone.Service.Abstract
{
    public interface IPaymentService
    {
        Task<bool> CheckPaymentAndAppointmentStatusAsync(int appointmentId);

        Task<Payment> CreatePaymentIntentAsync(int appointmentId, PaymentProvider provider);
        Task<bool> ConfirmPaymentAsync(int paymentId, string paymentMethodId);
        Task<Appointment> UpdateAppointmentStatusAfterPaymentAsync(int paymentId, bool isPaid);
        Task<bool> CancelAppointmentWithRefundAsync(int appointmentId, string cancellationReason);

        Task<bool> LogPaymentEventAsync(int paymentId, PaymentEventType eventType,
            string providerEventId = null, string eventData = null);
    }
}
