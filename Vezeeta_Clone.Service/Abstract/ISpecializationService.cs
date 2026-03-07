using Vezeeta_Clone.Data.Entities;

namespace Vezeeta_Clone.Service.Abstract
{
    public interface ISpecializationService
    {
        Task<string> CreateSpecialization(Specialization specialization);

        Task<bool> IsSpecializationExist(string specializationName);
    }
}
