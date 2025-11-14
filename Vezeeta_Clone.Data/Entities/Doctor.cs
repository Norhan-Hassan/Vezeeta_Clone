using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Vezeeta_Clone.Data.Entities.Enums;

namespace Vezeeta_Clone.Data.Entities
{
    public class Doctor
    {
        [Key]
        [ForeignKey("ApplicationUser")]
        public string AppUserID { get; set; }
        public ApplicationUser ApplicationUser { get; set; } = default!;
        public Title Title { get; set; }
        public string Description { get; set; }
        public int ExperienceInYears { get; set; }
        public int WaitingTimeInMinutes { get; set; }
        public string? Picture { get; set; }

        [ForeignKey("Specialization")]
        public int? SpecializationId { get; set; }
        public Specialization? Specialization { get; set; }
        public virtual ICollection<Review>? Reviews { get; set; } = new HashSet<Review>();
        public virtual ICollection<DoctorClinic>? DoctorClinics { get; set; } = new HashSet<DoctorClinic>();
        public ICollection<DoctorPatient> DoctorPatients { get; set; } = new HashSet<DoctorPatient>();
        public ICollection<DoctorAvailability> Availability { get; set; } = new HashSet<DoctorAvailability>();



    }
}
