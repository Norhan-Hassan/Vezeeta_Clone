using Vezeeta_Clone.Data.Entities.Enums;

namespace Vezeeta_Clone.Core.Features.Doctor.Queries.Results
{
    public class GetDoctorByIdResponse
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Title Title { get; set; }
        public string Description { get; set; }
        public int ExperienceInYears { get; set; }
        public string? Picture { get; set; }
        public string Specialization { get; set; }


    }
}
