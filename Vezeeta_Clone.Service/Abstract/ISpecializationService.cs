using Vezeeta_Clone.Data.Entities;

namespace Vezeeta_Clone.Service.Abstract
{
    public interface ISpecializationService
    {
        Task<string> CreateSpecialization(Specialization specialization);
        Task<string> UpdateSpecialization(Specialization specialization);
        Task<Specialization> GetSpecializationById(int id);

        Task<List<SubSpecialization>> GetSubSpecializationBySpecIDAsync(int specializationId);
        Task<bool> IsSpecializationExist(string specializationNameAr, string specializationNameEn, int? currentId = null);
    }
}
