using Vezeeta_Clone.Data.Entities;

namespace Vezeeta_Clone.Service.Abstract
{
    public interface ISlotService
    {
        Task<List<DoctorAvailabilitySlot>> GetDoctorAvailableSlotsAsync(string doctorId);
    }
}
