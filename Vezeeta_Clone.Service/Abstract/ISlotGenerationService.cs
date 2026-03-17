namespace Vezeeta_Clone.Service.Abstract
{
    public interface ISlotGenerationService
    {
        /// <summary>
        /// Generate slots for doctor availability
        /// - If DayOfWeek is set: generates weekly recurring slots (every Friday)
        /// - If Date is set: generates one-time slots for that date
        /// - Weeks are generated recursively until specified end date
        /// </summary>
        Task GenerateSlotsAsync(string doctorId, int weeks = 4);
    }
}
