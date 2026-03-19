using Microsoft.EntityFrameworkCore;
using Vezeeta_Clone.Data.Entities;
using Vezeeta_Clone.Data.Entities.Enums;
using Vezeeta_Clone.Infrastructure.InfrastructureBases;
using Vezeeta_Clone.Service.Abstract;

namespace Vezeeta_Clone.Service.Implementation
{
    public class SlotGenerationService : ISlotGenerationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDoctorService _doctorService;
        private readonly IDoctorAvailabilityService _availabilityService;

        public SlotGenerationService(IDoctorAvailabilityService scheduleService,
                                     IUnitOfWork unitOfWork,
                                    IDoctorService doctorService)
        {
            _availabilityService = scheduleService;
            _unitOfWork = unitOfWork;
            _doctorService = doctorService;
        }

        public async Task GenerateSlotsAsync(string doctorId, int weeks = 12)
        {
            var doctor = await _doctorService.GetDoctorByIdWithoutIncludesAsync(doctorId);

            if (doctor == null || doctor.ApplicationUser?.IsActive == false)
                return;

            var availabilities = await _availabilityService.GetDoctorAvailability(doctorId);
            if (!availabilities.Any()) return;

            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var endDate = today.AddDays(weeks * 7);

            var slotsToAdd = new List<DoctorAvailabilitySlot>();

            foreach (var availability in availabilities)
            {
                DateOnly startDate;

                if (availability.DayOfWeek.HasValue)
                {
                    var lastSlotDateForAvailability = await _unitOfWork._availabilitySlotRepo.GetTableNoTracking()
                        .Where(s => s.DoctorAvailabilityId == availability.ID)
                        .Where(s => s.Availability.frequency == ScheduleFrequency.Weekly)
                        .MaxAsync(s => (DateOnly?)s.Date);

                    if (lastSlotDateForAvailability != null)
                    {
                        startDate = lastSlotDateForAvailability.Value.AddDays(7);
                    }
                    else
                    {
                        int daysToAdd = ((int)availability.DayOfWeek.Value - (int)today.DayOfWeek + 7) % 7;
                        startDate = today.AddDays(daysToAdd);
                    }

                    var existingSlots = await _unitOfWork._availabilitySlotRepo.GetTableNoTracking()
                        .Where(s => s.DoctorAvailabilityId == availability.ID && s.Date <= endDate)
                       .ToListAsync();

                    slotsToAdd.AddRange(GenerateWeeklySlots(availability, startDate, endDate, existingSlots));
                }
                else if (availability.Date.HasValue)
                {
                    var existingSlots = await _unitOfWork._availabilitySlotRepo.GetTableNoTracking()
                        .Where(s => s.DoctorAvailabilityId == availability.ID)
                        .AnyAsync();

                    // Only generate if no slots exist yet for this one-time availability
                    if (!existingSlots)
                    {
                        slotsToAdd.AddRange(GenerateDateSlots(availability, availability.Date.Value));
                    }
                }
            }

            if (!slotsToAdd.Any()) return;

            var existingSlotKeys = new HashSet<(int, DateOnly, TimeOnly)>(
                await _unitOfWork._availabilitySlotRepo.GetTableNoTracking()
                    .Where(s => s.Availability.DoctorId == doctorId && s.Date <= endDate)
                    .Select(s => new ValueTuple<int, DateOnly, TimeOnly>(s.DoctorAvailabilityId, s.Date, s.StartTime))
                    .ToListAsync()
            );

            var filteredSlots = slotsToAdd
                .Where(s => !existingSlotKeys.Contains((s.DoctorAvailabilityId, s.Date, s.StartTime)))
                .ToList();

            if (!filteredSlots.Any()) return;

            await _unitOfWork._availabilitySlotRepo.AddRangeAsync(filteredSlots);
            await _unitOfWork.SaveChangesAsync();
        }

        private List<DoctorAvailabilitySlot> GenerateWeeklySlots(DoctorAvailability availability, DateOnly startDate, DateOnly endDate, List<DoctorAvailabilitySlot> existingSlots)
        {
            var slots = new List<DoctorAvailabilitySlot>();

            var lastWeeklyDate = existingSlots
                               .Where(s => !s.IsDeleted)
                               .OrderBy(s => s.Date)
                               .Select(s => s.Date)
                               .LastOrDefault();

            // Start from the day after the last weekly slot, or from startDate if none exist
            var baseDate = lastWeeklyDate != default ? lastWeeklyDate.AddDays(7) : startDate;

            // Find the first day matching the DayOfWeek
            int daysToAdd = ((int)availability.DayOfWeek!.Value - (int)baseDate.DayOfWeek + 7) % 7;
            var firstDay = baseDate.AddDays(daysToAdd);

            var current = firstDay;

            while (current <= endDate)
            {
                slots.AddRange(GenerateDateSlots(availability, current));
                current = current.AddDays(7);
            }

            return slots;
        }

        private List<DoctorAvailabilitySlot> GenerateDateSlots(DoctorAvailability availability, DateOnly date)
        {
            var slots = new List<DoctorAvailabilitySlot>();

            var current = availability.StartTime;

            while (true)
            {
                var slotEnd = current.AddMinutes(availability.Duration);

                // stop if slot start already >= end time
                if (current >= availability.EndTime)
                    break;

                // if slot exceeds end time
                if (slotEnd > availability.EndTime)
                {
                    var remaining =
                        availability.EndTime.ToTimeSpan() -
                        current.ToTimeSpan();

                    if (remaining.TotalMinutes < availability.Duration)
                        break;

                    slotEnd = availability.EndTime;
                }

                slots.Add(new DoctorAvailabilitySlot
                {
                    DoctorAvailabilityId = availability.ID,
                    Date = date,
                    StartTime = current,
                    EndTime = slotEnd,
                    IsBooked = false
                });

                current = slotEnd;
            }

            return slots;
        }

        public async Task MaintainFutureSlotsAsync(int requiredWeeks = 12)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            // get all doctors that have availability
            var doctors = await _availabilityService.GetDoctorsWithAvailability();

            foreach (var doctorId in doctors)
            {
                var availabilities = await _availabilityService.GetDoctorAvailability(doctorId);

                // Only maintain slots for doctors with weekly availabilities
                var hasWeeklyAvailability = availabilities.Any(a => a.frequency == ScheduleFrequency.Weekly);

                if (!hasWeeklyAvailability)
                    continue;

                // get last slot date for doctor
                var lastSlotDate = await _unitOfWork._availabilitySlotRepo.GetTableNoTracking()
                    .Include(s => s.Availability)
                    .Where(s => s.Availability.DoctorId == doctorId)
                    .Where(s => s.Availability.frequency == ScheduleFrequency.Weekly)
                    .MaxAsync(s => (DateOnly?)s.Date);

                if (lastSlotDate == null)
                {
                    // doctor has weekly availability but no slots generated yet
                    await GenerateSlotsAsync(doctorId, requiredWeeks);
                    continue;
                }

                var remainingDays = lastSlotDate.Value.DayNumber - today.DayNumber;

                if (remainingDays < requiredWeeks * 7)
                {
                    await GenerateSlotsAsync(doctorId, requiredWeeks);
                }
            }
        }
    }
}