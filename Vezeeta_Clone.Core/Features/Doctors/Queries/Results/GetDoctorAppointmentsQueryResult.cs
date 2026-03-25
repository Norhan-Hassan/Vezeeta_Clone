namespace Vezeeta_Clone.Core.Features.Doctors.Queries.Results
{
    public class GetDoctorAppointmentsQueryResult
    {
        public int AppointmentId { get; set; }//
        public string ActualPatientName { get; set; }//
        public string ActualPatientPhone { get; set; }//
        public DateOnly Date { get; set; } //
        public TimeOnly StartTime { get; set; }// available slots include
        public TimeOnly EndTime { get; set; }// available slots include
        public string Status { get; set; }//
        public GetDoctorAppointmentsQueryResult(int appointmentId, string patientName,
            string patientPhone, DateOnly date, TimeOnly startTime, TimeOnly endTime, string? status = null)
        {
            AppointmentId = appointmentId;
            ActualPatientName = patientName;
            ActualPatientPhone = patientPhone;
            Date = date;
            StartTime = startTime;
            EndTime = endTime;
            Status = status;
        }
    }
}
