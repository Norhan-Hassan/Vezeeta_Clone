using Microsoft.EntityFrameworkCore;
using Vezeeta_Clone.Data.Entities;
using Vezeeta_Clone.Infrastructure.Abstract;
using Vezeeta_Clone.Service.Abstract;

namespace Vezeeta_Clone.Service.Implementation
{
    public class SlotService : ISlotService
    {
        private readonly IDoctorAvailabilitySlotRepo _availabilitySlotRepo;
        public SlotService(IDoctorAvailabilitySlotRepo availabilitySlotRepo)
        {
            _availabilitySlotRepo = availabilitySlotRepo;
        }

        public async Task<List<DoctorAvailabilitySlot>> GetDoctorAvailableSlotsAsync(string doctorId)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var nowTime = TimeOnly.FromDateTime(DateTime.UtcNow);

            var slots = await _availabilitySlotRepo.GetTableNoTracking()
                                .Where(s => s.Availability.DoctorId == doctorId && !s.IsBooked &&
                                                        (s.Date > today ||
                                                        (s.Date == today && s.StartTime > nowTime)))
                                .OrderBy(s => s.Date)
                                .ThenBy(s => s.StartTime)
                                .ToListAsync();

            return slots;
        }


    }
}
