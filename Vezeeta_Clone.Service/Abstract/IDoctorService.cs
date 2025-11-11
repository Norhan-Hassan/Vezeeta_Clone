using Vezeeta_Clone.Data.Entities;

namespace Vezeeta_Clone.Service.Abstract
{
    public interface IDoctorService
    {
        Task<Doctor> GetDoctorByIDAsync(string id);
    }
}
