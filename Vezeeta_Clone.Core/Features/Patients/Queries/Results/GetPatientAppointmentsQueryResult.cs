namespace Vezeeta_Clone.Core.Features.Patients.Queries.Results
{
    public class GetPatientAppointmentsQueryResult
    {
        public int AppointmentId { get; set; }//
        public string ActualPatientName { get; set; }//
        public string ActualPatientPhone { get; set; }//
        public string DoctorName { get; set; } // query
        public DateOnly Date { get; set; } //
        public TimeOnly StartTime { get; set; }//query
        public string Status { get; set; }//

        public GetPatientAppointmentsQueryResult(int appointmentId, string patientName, string patientPhone, string doctorName, DateOnly date, TimeOnly startTime, string status)
        {
            Status = status;
            StartTime = startTime;
            Date = date;
            DoctorName = doctorName;
            ActualPatientPhone = patientPhone;
            ActualPatientName = patientName;
            AppointmentId = appointmentId;


        }
    }
}
