namespace Vezeeta_Clone.Service.Abstract
{
    public interface IPaymentService
    {
        Task<bool> CheckPaymentAndAppointmentStatusAsync(int appointmentId);
    }
}
