namespace Vezeeta_Clone.Core.Features.Doctors.Queries.Results
{
    public class GetDoctorExaminationDetailsQueryResult
    {
        public decimal Price { get; set; }
        public int WaitingTimeInMinutes { get; set; }
        public string ClinicCity { get; set; }
        public string ClinicRegion { get; set; }
        public string ClinicAddress { get; set; }
    }
}
