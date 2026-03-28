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
        public string? Description { get; set; }
        public int ExperienceInYears { get; set; }
        public string? Picture { get; set; }


        [ForeignKey("University")]
        public int UniversityId { get; set; }
        public University University { get; set; }

        public bool IsProfileComplete { get; set; } = false;

        [ForeignKey("Specialization")]
        public int? SpecializationId { get; set; }
        public Specialization? Specialization { get; set; }
        public Clinic? Clinic { get; set; }

        public ICollection<SubSpecialization>? SubSpecializations { get; set; } = new HashSet<SubSpecialization>();
        public ICollection<Review>? Reviews { get; set; } = new HashSet<Review>();

        public ICollection<DoctorPatient> DoctorPatients { get; set; } = new HashSet<DoctorPatient>();
        public ICollection<DoctorAvailability> Availability { get; set; } = new HashSet<DoctorAvailability>();
        public ICollection<Appointment>? Appointments { get; set; } = new HashSet<Appointment>();
    }
}
