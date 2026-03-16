using System.ComponentModel.DataAnnotations.Schema;

namespace Vezeeta_Clone.Data.Entities
{
    public class Clinic : BaseEntity
    {
        public string Name { get; set; }
        public string Address { get; set; }

        [ForeignKey("Region")]
        public int RegionId { get; set; }
        public Region Region { get; set; }

        [ForeignKey("ClinicLocation")]
        public int LocationId { get; set; }
        public Location ClinicLocation { get; set; }
        public string PhoneNumber { get; set; }

        public int? WaitingTimeInMinutes { get; set; }
        public decimal Price { get; set; }

        [ForeignKey("Doctor")]
        public string? DoctorId { get; set; }
        public Doctor? Doctor { get; set; }
    }
}
