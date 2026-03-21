namespace Vezeeta_Clone.Core.Features.Doctors.Queries.Results
{
    public class DoctorAvailableSlotQueryResult
    {
        public string Date { get; set; }
        public string DayName { get; set; }
        public List<SlotInfo> StartTimes { get; set; }
    }
    public class SlotInfo
    {
        public string StartTime { get; set; }
        public int Id { get; set; }
    }
}
