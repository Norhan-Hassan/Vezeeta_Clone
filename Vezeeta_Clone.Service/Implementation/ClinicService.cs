using Microsoft.EntityFrameworkCore;
using Vezeeta_Clone.Data.Entities;
using Vezeeta_Clone.Infrastructure.Abstract;
using Vezeeta_Clone.Service.Abstract;

namespace Vezeeta_Clone.Service.Implementation
{
    public class ClinicService : IClinicService
    {
        private readonly IClinicRepo _clinicRepo;
        private readonly IDoctorRepo _doctorRepo;
        public ClinicService(IClinicRepo clinicRepo, IDoctorRepo doctorRepo)
        {
            _clinicRepo = clinicRepo;
            _doctorRepo = doctorRepo;
        }

        public async Task<bool> IsClinicExist(string doctorId)
        {
            //check if the doctor already has a clinic
            return await _clinicRepo.GetTableNoTracking()
                                    .Include(c => c.Doctor)
                                     .AnyAsync(c => c.Doctor.AppUserID == doctorId);
        }

        public async Task RegisterClinicToDoctor(Clinic clinic, string doctorId)
        {
            var doctor = await _doctorRepo.GetByStringIdAsync(doctorId);

            var transaction = _clinicRepo.BeginTransaction();
            try
            {

                await _clinicRepo.AddAsync(clinic);
                await _clinicRepo.SaveChangesAsync();
                doctor.ClinicId = clinic.ID;
                await _doctorRepo.UpdateAsync(doctor);
                await _doctorRepo.SaveChangesAsync();
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
