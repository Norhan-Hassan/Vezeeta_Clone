using Microsoft.EntityFrameworkCore;
using Vezeeta_Clone.Data.Entities;
using Vezeeta_Clone.Infrastructure.InfrastructureBases;
using Vezeeta_Clone.Service.Abstract;

namespace Vezeeta_Clone.Service.Implementation
{
    public class SpecializationService : ISpecializationService
    {
        private readonly IUnitOfWork _unitOfWork;
        public SpecializationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }

        public async Task<string> CreateSpecialization(Specialization specialization)
        {
            if (specialization == null)
                throw new ArgumentNullException(nameof(specialization));

            await _unitOfWork._specializationRepo.AddAsync(specialization);
            await _unitOfWork.SaveChangesAsync();
            return "success";
        }
        public async Task<string> UpdateSpecialization(Specialization specialization)
        {
            if (specialization == null)
                throw new ArgumentNullException(nameof(specialization));
            await _unitOfWork._specializationRepo.UpdateAsync(specialization);
            await _unitOfWork.SaveChangesAsync();
            return "success";
        }
        public async Task<bool> IsSpecializationExist(string specializationNameAr, string specializationNameEn, int? currentId = null)
        {
            return await _unitOfWork._specializationRepo.GetTableNoTracking()
                                            .AnyAsync(x => (x.NameEn == specializationNameEn ||
                                                      x.NameAr == specializationNameAr) && x.ID != currentId);
        }

        public async Task<Specialization> GetSpecializationById(int id)
        {
            var specialization = await _unitOfWork._specializationRepo.GetByIntIdAsync(id);
            if (specialization == null)
                throw new KeyNotFoundException($"Specialization with ID {id} not found");
            return specialization;
        }

        public async Task<List<SubSpecialization>> GetSubSpecializationBySpecIDAsync(int specializationId)
        {
            var subSpecializations = await _unitOfWork._specializationRepo.GetTableNoTracking()
                                                        .Where(s => s.ID == specializationId)
                                                        .SelectMany(s => s.SubSpecializations)
                                                        .ToListAsync();
            return subSpecializations;
        }

        public Task<List<Specialization>> GetSpecializationsAsync()
        {
            var specializations = _unitOfWork._specializationRepo.GetTableNoTracking()
                                                     //.OrderByDescending(s => s.Doctors!.Count) //later when i have doctors data to order by the number of doctors in each specialization
                                                     .ToListAsync();
            return specializations;
        }
    }
}
