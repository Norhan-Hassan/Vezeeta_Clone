using Vezeeta_Clone.Data.Entities;

namespace Vezeeta_Clone.Service.Abstract
{
    public interface IClinicService
    {
        Task<bool> IsClinicExist(string doctorId);
        Task RegisterClinicToDoctor(Clinic clinic, string doctorId);
    }
}
