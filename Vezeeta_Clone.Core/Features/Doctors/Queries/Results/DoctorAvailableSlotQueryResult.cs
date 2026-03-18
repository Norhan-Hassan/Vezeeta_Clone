namespace Vezeeta_Clone.Core.Features.Doctors.Queries.Results
{
    public class DoctorAvailableSlotQueryResult
    {
        public string Date { get; set; }
        public string DayName { get; set; }
        public List<string> StartTimes { get; set; }
    }
}
