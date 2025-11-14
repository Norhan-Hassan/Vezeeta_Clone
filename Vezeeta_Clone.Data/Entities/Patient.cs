using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vezeeta_Clone.Data.Entities
{
    public class Patient
    {
        [Key]
        [ForeignKey("ApplicationUser")]
        public string AppUserID { get; set; }
        public ApplicationUser ApplicationUser { get; set; } = default!;
        public DateTime DateOfBirth { get; set; }
        public ICollection<Appointment>? Appointments { get; set; } = new HashSet<Appointment>();
        public ICollection<Review>? Reviews { get; set; } = new HashSet<Review>();
        public ICollection<DoctorPatient> DoctorPatients { get; set; } = new HashSet<DoctorPatient>();


    }
}
