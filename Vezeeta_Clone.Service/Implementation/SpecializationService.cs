using Microsoft.EntityFrameworkCore;
using Vezeeta_Clone.Data.Entities;
using Vezeeta_Clone.Infrastructure.Abstract;
using Vezeeta_Clone.Service.Abstract;

namespace Vezeeta_Clone.Service.Implementation
{
    public class SpecializationService : ISpecializationService
    {
        private readonly ISpecializationRepo _specializationRepo;
        public SpecializationService(ISpecializationRepo specializationRepo)
        {
            _specializationRepo = specializationRepo;

        }

        public async Task<string> CreateSpecialization(Specialization specialization)
        {
            if (specialization == null)
                throw new ArgumentNullException(nameof(specialization));

            await _specializationRepo.AddAsync(specialization);
            await _specializationRepo.SaveChangesAsync();
            return "success";
        }

        public async Task<bool> IsSpecializationExist(string specializationName)
        {
            var result = await _specializationRepo.GetTableNoTracking().AnyAsync(x => x.Name == specializationName);
            if (result == true)
                return true;
            else
                return false;
        }
    }
}
