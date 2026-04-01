namespace Vezeeta_Clone.Core.Features.Patients.Queries.Results
{
    public class GetPatientProfileQueryResult
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string DateOfBirth { get; set; }
        public string? Blood_Type { get; set; }

    }
}
