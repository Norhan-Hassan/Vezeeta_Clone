using Microsoft.EntityFrameworkCore;
using Vezeeta_Clone.Data.Entities;
using Vezeeta_Clone.Infrastructure.InfrastructureBases;
using Vezeeta_Clone.Service.Abstract;

namespace Vezeeta_Clone.Service.Implementation
{
    public class SlotService : ISlotService
    {
        private readonly IUnitOfWork _unitOfWork;
        public SlotService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<DoctorAvailabilitySlot>> GetDoctorAvailableSlotsAsync(string doctorId)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var nowTime = TimeOnly.FromDateTime(DateTime.UtcNow);

            var slots = await _unitOfWork._availabilitySlotRepo.GetTableNoTracking()
                                .Where(s => s.Availability.DoctorId == doctorId && !s.IsBooked &&

                                                        (s.Date > today ||
                                                        (s.Date == today && s.StartTime > nowTime)))
                                .OrderBy(s => s.Date)
                                .ThenBy(s => s.StartTime)
                                .ToListAsync();

            return slots;
        }

        public async Task<bool> LockSlotAsync(int slotId, string doctorId, string lockedReson)
        {
            var slot = await _unitOfWork._availabilitySlotRepo.GetTableNoTracking()
                                                        .Include(s => s.Availability)
                                                        .Where(s => s.ID == slotId)
                                                        .FirstOrDefaultAsync();

            if (slot.Availability.DoctorId == doctorId && slot.IsBooked == false)
            {
                slot.LockedReason = lockedReson;
                slot.IsDeleted = true;
                await _unitOfWork._availabilitySlotRepo.UpdateAsync(slot);
                var result = await _unitOfWork.SaveChangesAsync();
                if (result > 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}

