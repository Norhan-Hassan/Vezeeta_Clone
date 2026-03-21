using Vezeeta_Clone.Data.Entities;

namespace Vezeeta_Clone.Service.Abstract
{
    public interface IAppointmentService
    {
        Task<int> BookAppointmentAsync(Appointment appointment, string patientId);
        Task<bool> CompleteAppointmentAsync(Appointment appointment);
        Task<Appointment> GetAppointmentByIdAsync(int appointmentId);

    }

}
