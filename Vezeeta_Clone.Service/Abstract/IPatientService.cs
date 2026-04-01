using Vezeeta_Clone.Data.Entities;

namespace Vezeeta_Clone.Service.Abstract
{
    public interface IPatientService
    {
        Task<Patient> GetPatientByAppointmentId(int appointmentId);
        Task<Patient> GetPatientByIdAsync(string patientId);
    }
}
