using Vezeeta_Clone.Data.Entities;

namespace Vezeeta_Clone.Service.Abstract
{
    public interface IDoctorAvailabilityService
    {
        Task<string> SetDoctorAvailabilityAsync(string doctorId, DoctorAvailability schedule);
        Task<List<DoctorAvailability>> GetDoctorAvailability(string doctorId);
        Task<List<string>> GetDoctorsWithAvailability();


    }
}
