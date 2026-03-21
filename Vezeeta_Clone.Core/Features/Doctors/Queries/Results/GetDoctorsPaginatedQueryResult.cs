namespace Vezeeta_Clone.Core.Features.Doctors.Queries.Results
{
    public class GetDoctorsPaginatedQueryResult
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string? Picture { get; set; }
        public string? Specialization { get; set; }
        public int? ExperienceInYears { get; set; }
        public string? ClinicRegion { get; set; }


        public GetDoctorsPaginatedQueryResult(string id, string fullName, int experienceInYears, string specialization, string? clinicRegion, string? picture)
        {
            Id = id;
            FullName = fullName;
            Picture = picture;
            ExperienceInYears = experienceInYears;
            Specialization = specialization;
            ClinicRegion = clinicRegion;
        }
    }
}
