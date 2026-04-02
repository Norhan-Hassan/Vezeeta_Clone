using Microsoft.EntityFrameworkCore;
using Serilog;
using Vezeeta_Clone.Data.Entities;
using Vezeeta_Clone.Infrastructure.InfrastructureBases;
using Vezeeta_Clone.Service.Abstract;

namespace Vezeeta_Clone.Service.Implementation
{
    public class ClinicService : IClinicService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ClinicService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> IsClinicExist(string doctorId)
        {
            //check if the doctor already has a clinic
            return await _unitOfWork._clinicRepo.GetTableNoTracking()
                                    .Include(c => c.Doctor)
                                     .AnyAsync(c => c.Doctor.AppUserID == doctorId);
        }

        public async Task RegisterClinicToDoctor(Clinic clinic, string doctorId)
        {
            var doctor = await _unitOfWork._doctorRepo.GetByStringIdAsync(doctorId);

            var transaction = _unitOfWork._clinicRepo.BeginTransaction();
            try
            {
                if (doctor.IsProfileComplete == false)
                {
                    throw new Exception("CompleteProfile");
                }
                await _unitOfWork._clinicRepo.AddAsync(clinic);
                clinic.DoctorId = doctor.AppUserID;
                await _unitOfWork._doctorRepo.UpdateAsync(doctor);

                await _unitOfWork.SaveChangesAsync();
                transaction.Commit();
            }
            catch (Exception ex)
            {

                transaction.Rollback();
                Log.Error(ex.Message);
                throw;
            }
        }
        public async Task<Clinic?> GetClinicByIdAsync(int clinicId)
        {
            return await _unitOfWork._clinicRepo.GetTableNoTracking()
                                    .Include(c => c.Doctor)
                                     .FirstOrDefaultAsync(c => c.ID == clinicId);
        }
        public async Task SaveClinicImages(string doctorId, List<ClinicImage>? ClinicImages)
        {
            var clinic = await _unitOfWork._clinicRepo.GetTableNoTracking()
                                     .Include(c => c.Doctor)
                                     .FirstOrDefaultAsync(c => c.Doctor.AppUserID == doctorId);


            foreach (var image in ClinicImages)
            {
                image.clinicId = clinic.ID;
                await _unitOfWork._clinicImageRepo.AddAsync(image);
            }

            await _unitOfWork._clinicRepo.UpdateAsync(clinic);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<List<ClinicImage?>> GetClinicImagesByClinicIdAsync(int clinicId)
        {
            var clinic = await _unitOfWork._clinicRepo.GetTableNoTracking()
                                     .FirstOrDefaultAsync(c => c.ID == clinicId);
            if (clinic != null)
            {
                var clinicImages = await _unitOfWork._clinicImageRepo.GetTableNoTracking()
                                            .Where(ci => ci.clinicId == clinicId)
                                            .ToListAsync();
                if (clinicImages != null)
                    return clinicImages;
            }
            return new List<ClinicImage?>();
        }
    }
}
