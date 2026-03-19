using Microsoft.EntityFrameworkCore;
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

                await _unitOfWork._clinicRepo.AddAsync(clinic);
                clinic.DoctorId = doctor.AppUserID;
                await _unitOfWork._doctorRepo.UpdateAsync(doctor);

                await _unitOfWork.SaveChangesAsync();
                transaction.Commit();
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework like Serilog, NLog, etc.)
                Console.WriteLine($"An error occurred while adding the clinic: {ex.Message}");
                transaction.Rollback();
                throw; // Rethrow the exception to be handled by the caller
            }
        }
    }
}
