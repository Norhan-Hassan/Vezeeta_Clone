using Vezeeta_Clone.Data.Entities;
using Vezeeta_Clone.Data.Entities.Enums;

namespace Vezeeta_Clone.Service.Abstract
{
    public interface IAppointmentService
    {
        Task<int> BookAppointmentAsync(Appointment appointment, string patientId);
        Task<bool> CompleteAppointmentAsync(Appointment appointment);
        Task<Appointment> GetAppointmentByIdAsync(int appointmentId);
        IQueryable<Appointment> GetPatientAppointmentsAsync(string patientId, AppointmentStatus? appointmentStatus);
        IQueryable<Appointment> GetDoctorAppointmentsAsync(string doctorId, AppointmentStatus? appointmentStatus);
    }

}
