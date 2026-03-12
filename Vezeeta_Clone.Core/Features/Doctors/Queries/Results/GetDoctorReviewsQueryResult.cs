namespace Vezeeta_Clone.Core.Features.Doctors.Queries.Results
{
    public class GetDoctorReviewsQueryResult
    {
        public double Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? PatientName { get; set; }
        public int PatientAge { get; set; }

        public GetDoctorReviewsQueryResult(double rating, string? comment, DateTime createdAt, string? patientName, int patientAge)
        {
            Rating = rating;
            Comment = comment;
            CreatedAt = createdAt;
            PatientName = patientName;
            PatientAge = patientAge;
        }
    }
}
