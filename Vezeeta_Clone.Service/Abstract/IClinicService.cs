using Vezeeta_Clone.Data.Entities;

namespace Vezeeta_Clone.Service.Abstract
{
    public interface IClinicService
    {
        Task<Clinic?> GetClinicByIdAsync(int clinicId);
        Task<List<ClinicImage?>> GetClinicImagesByClinicIdAsync(int clinicId);
        Task SaveClinicImages(string doctorId, List<ClinicImage>? ClinicImages);
        Task<bool> IsClinicExist(string doctorId);
        Task RegisterClinicToDoctor(Clinic clinic, string doctorId);
    }
}
