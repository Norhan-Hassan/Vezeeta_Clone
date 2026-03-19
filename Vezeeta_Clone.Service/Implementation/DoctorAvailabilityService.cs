using Microsoft.EntityFrameworkCore;
using Vezeeta_Clone.Data.Entities;
using Vezeeta_Clone.Data.Entities.Enums;
using Vezeeta_Clone.Infrastructure.InfrastructureBases;
using Vezeeta_Clone.Service.Abstract;
using Vezeeta_Clone.Service.BackgroundJobServices.Abstract;

namespace Vezeeta_Clone.Service.Implementation
{
    public class DoctorAvailabilityService : IDoctorAvailabilityService
    {
        private readonly IDoctorService _doctorService;
        private readonly IBackgroundJobService _backgroundJobService;
        private readonly IUnitOfWork _unitOfWork;

        public DoctorAvailabilityService(IDoctorService doctorService,
                                        IUnitOfWork unitOfWork,
                                    IBackgroundJobService backgroundJobService)
        {
            _doctorService = doctorService;
            _backgroundJobService = backgroundJobService;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<DoctorAvailability>> GetDoctorAvailability(string doctorId)
        {

            var availabilities = await _unitOfWork._doctorAvailabilityRepo.GetTableNoTracking()
                                                       .Where(a => a.DoctorId == doctorId)
                                                       .ToListAsync();
            return availabilities;

        }
        public async Task<List<string>> GetDoctorsWithAvailability()
        {
            var doctorIds = await _unitOfWork._doctorAvailabilityRepo
                                        .GetTableNoTracking()
                                        .Select(a => a.DoctorId)
                                        .Distinct()
                                        .ToListAsync();

            return doctorIds;
        }
        private async Task<bool> CheckAvailabilityOverlap(string doctorId, DoctorAvailability schedule)
        {
            var exist = await _unitOfWork._doctorAvailabilityRepo.GetTableNoTracking()
                                                       .Where(a => a.DoctorId == doctorId)
                                                       .Where(a =>
                                                             (a.Date == schedule.Date && schedule.Date != null) ||
                                                             (a.Date == null && schedule.Date == null && a.DayOfWeek == schedule.DayOfWeek)
                                                       ).Where(a => a.StartTime < schedule.EndTime && a.EndTime > schedule.StartTime)
                                                       .AnyAsync();
            return exist;
        }
        private async Task<bool> CheckAvailabityType(string doctorId, DoctorAvailability schedule)
        {
            if (schedule.type == AvailabilityMethod.Offline)
            {
                Doctor doctor = await _doctorService.GetDoctorWithClinicByIDAsync(doctorId);
                if (doctor != null && doctor.Clinic != null && doctor.Clinic.DoctorId == doctorId)
                {
                    return true; // The doctor has a clinic, so they can set offline availability
                }
                return false;
            }
            return true; //doesn't mattter
        }

        public async Task<string> SetDoctorAvailabilityAsync(string doctorId, DoctorAvailability schedule)
        {
            if (await CheckAvailabityType(doctorId, schedule))
            {
                if (await CheckAvailabilityOverlap(doctorId, schedule))
                {
                    return "overlapping";
                }

                schedule.DoctorId = doctorId;
                if (schedule.DayOfWeek != null)
                {
                    schedule.frequency = ScheduleFrequency.Weekly;
                }
                else
                {
                    schedule.frequency = ScheduleFrequency.OneTime;
                }
                await _unitOfWork._doctorAvailabilityRepo.AddAsync(schedule);
                await _unitOfWork.SaveChangesAsync();

                //background job to generate slots
                var jobId = await _backgroundJobService.EnqueueAsync<ISlotGenerationService>(
                                        x => x.GenerateSlotsAsync(schedule.DoctorId, 4));
                return "success";
            }
            return "fail";
        }
    }
}
