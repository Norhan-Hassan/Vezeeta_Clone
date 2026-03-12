namespace Vezeeta_Clone.Core.Features.Doctors.Queries.Results
{
    public class GetDoctorDetailsQueryResult
    {
        public string FullName { get; set; }
        public string Title { get; set; }
        public string University { get; set; }
        public string Description { get; set; }
        public string? Specialization { get; set; }
        public string[]? SubSpecializations { get; set; }
        public string? Picture { get; set; }
        public double AverageRating { get; set; }
        public int TotalRating { get; set; }
    }
}
