using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Vezeeta_Clone.Data.Entities;
using Vezeeta_Clone.Infrastructure.Abstract;
using Vezeeta_Clone.Service.Abstract;

namespace Vezeeta_Clone.Service.Implementation
{
    public class SlotGenerationService : ISlotGenerationService
    {
        private readonly IDoctorAvailabilityRepo _availabilityRepo;
        private readonly IDoctorAvailabilitySlotRepo _slotRepo;
        private readonly IDoctorService _doctorService;
        private readonly IDoctorAvailabilityService _availabilityService;
        private readonly ILogger<SlotGenerationService> _logger;

        public SlotGenerationService(
            IDoctorAvailabilityRepo availabilityRepo,
            IDoctorAvailabilitySlotRepo slotRepo,
            IDoctorAvailabilityService scheduleService,
            IDoctorService doctorService,
            ILogger<SlotGenerationService> logger)
        {
            _availabilityRepo = availabilityRepo;
            _slotRepo = slotRepo;
            _availabilityService = scheduleService;
            _doctorService = doctorService;
            _logger = logger;
        }

        public async Task GenerateSlotsAsync(string doctorId, int weeks = 4)
        {
            try
            {
                _logger.LogInformation($"Starting slot generation for doctor: {doctorId}, weeks: {weeks}");

                // FIX #5: Add null check
                var doctor = await _doctorService.GetDoctorByWithoutIncludesAsync(doctorId);
                if (doctor == null)
                {
                    _logger.LogWarning($"Doctor not found with ID: {doctorId}");
                    return;
                }

                var availabilities = await _availabilityService.GetDoctorAvailability(doctorId);

                if (!availabilities.Any())
                {
                    _logger.LogInformation($"No availability patterns found for doctor: {doctorId}");
                    return;
                }

                var slotsToAdd = new List<DoctorAvailabilitySlot>();
                var today = DateOnly.FromDateTime(DateTime.UtcNow);  // Use UTC for consistency
                var endDate = today.AddDays(weeks * 7);

                _logger.LogInformation($"Generating slots from {today} to {endDate} for {availabilities.Count} availability patterns");

                foreach (var availability in availabilities)
                {
                    // FIX #2: Validate time range
                    if (availability.StartTime >= availability.EndTime)
                    {
                        _logger.LogError($"Invalid time range for availability ID {availability.ID}: " +
                            $"StartTime ({availability.StartTime}) >= EndTime ({availability.EndTime})");
                        continue;  // Skip this pattern
                    }

                    // Weekly recurring schedule (DayOfWeek set)
                    if (availability.DayOfWeek.HasValue)
                    {
                        _logger.LogDebug($"Generating weekly slots for DayOfWeek: {availability.DayOfWeek}");
                        slotsToAdd.AddRange(GenerateWeeklySlots(availability, today, endDate));
                    }
                    // One-time special date
                    else if (availability.Date.HasValue)
                    {
                        _logger.LogDebug($"Generating one-time slots for date: {availability.Date}");
                        slotsToAdd.AddRange(GenerateDateSlots(availability));
                    }
                    // FIX #1: Handle case where both are NULL (all days pattern)
                    else
                    {
                        _logger.LogDebug($"Generating slots for all days from {today} to {endDate}");
                        var current = today;
                        while (current <= endDate)
                        {
                            slotsToAdd.AddRange(GenerateDateSlots(availability, current));
                            current = current.AddDays(1);
                        }
                    }
                }

                _logger.LogInformation($"Generated {slotsToAdd.Count} total slots before duplicate check");

                // FIX #4 & #5: Improved duplicate checking with better performance and null safety
                var existingSlotKeys = new HashSet<(int, DateOnly, TimeOnly)>(
                    await _slotRepo.GetTableNoTracking()
                        .Where(s => s.Availability != null && s.Availability.DoctorId == doctorId)
                        .Select(s => new ValueTuple<int, DateOnly, TimeOnly>(s.DoctorAvailabilityId, s.Date, s.StartTime))
                        .ToListAsync());

                var filteredSlots = slotsToAdd
                    .Where(s => !existingSlotKeys.Contains((s.DoctorAvailabilityId, s.Date, s.StartTime)))
                    .ToList();

                _logger.LogInformation($"After duplicate removal: {filteredSlots.Count} new slots to add");

                // FIX #6: Add transaction control
                if (filteredSlots.Any())
                {
                    try
                    {
                        await _slotRepo.AddRangeAsync(filteredSlots);
                        await _slotRepo.SaveChangesAsync();

                        _logger.LogInformation(
                            $"Successfully generated and saved {filteredSlots.Count} slots for doctor: {doctorId}");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex,
                            $"Failed to save slots for doctor {doctorId}. Rolled back {filteredSlots.Count} slots.");
                        throw;
                    }
                }
                else
                {
                    _logger.LogInformation($"No new slots to add for doctor: {doctorId} (all were duplicates)");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error generating slots for doctor {doctorId}");
                throw;
            }
        }

        private List<DoctorAvailabilitySlot> GenerateWeeklySlots(
            DoctorAvailability availability,
            DateOnly startDate,
            DateOnly endDate)
        {
            var slots = new List<DoctorAvailabilitySlot>();
            var current = startDate;

            // Find first occurrence of target day of week (with safety limit of 7 days)
            // Calculate days until target day of week
            int daysToAdd = ((int)availability.DayOfWeek - (int)current.DayOfWeek + 7) % 7;
            if (daysToAdd == 0 && current < startDate.AddDays(1))
            {
                daysToAdd = 0;  // Start today if it's the target day
            }
            current = current.AddDays(daysToAdd);

            if (current.DayOfWeek != availability.DayOfWeek)
            {
                _logger.LogWarning($"Could not find matching day of week {availability.DayOfWeek} within a week");
                return slots;
            }

            _logger.LogDebug($"First occurrence of {availability.DayOfWeek} found on {current}");

            // Generate slots for each week
            int weekCount = 0;
            while (current <= endDate)
            {
                slots.AddRange(GenerateDateSlots(availability, current));
                current = current.AddDays(7); // Next week
                weekCount++;
            }

            _logger.LogDebug($"Generated slots for {weekCount} weeks");

            return slots;
        }

        private List<DoctorAvailabilitySlot> GenerateDateSlots(
            DoctorAvailability availability,
            DateOnly? specificDate = null)
        {
            var slots = new List<DoctorAvailabilitySlot>();
            var date = specificDate ?? availability.Date;

            if (!date.HasValue)
            {
                _logger.LogWarning($"No date available for availability ID {availability.ID}");
                return slots;
            }

            var current = availability.StartTime;
            int slotCount = 0;

            while (current < availability.EndTime)
            {
                var slotEnd = current.AddMinutes(availability.Duration);

                // FIX #3: Handle edge case where slot would exceed end time
                if (slotEnd > availability.EndTime)
                {
                    // Option: Create slot only if it would be at least half the duration
                    var remainingTime = availability.EndTime - current;
                    // Use double division to avoid integer division precision loss
                    if (remainingTime.TotalMinutes >= (availability.Duration / 2.0))
                    {
                        slotEnd = availability.EndTime;
                    }
                    else
                    {
                        // Skip the last short slot
                        break;
                    }
                }

                slots.Add(new DoctorAvailabilitySlot
                {
                    StartTime = current,
                    EndTime = slotEnd,
                    Date = date.Value,
                    IsBooked = false,
                    DoctorAvailabilityId = availability.ID
                });

                current = slotEnd;
                slotCount++;
            }

            _logger.LogDebug($"Generated {slotCount} slots for date {date}");

            return slots;
        }
    }
}


