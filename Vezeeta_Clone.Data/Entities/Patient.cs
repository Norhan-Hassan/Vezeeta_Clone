using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Vezeeta_Clone.Data.Entities.Enums;

namespace Vezeeta_Clone.Data.Entities
{
    public class Patient
    {
        [Key]
        [ForeignKey("ApplicationUser")]
        public string AppUserID { get; set; }
        public ApplicationUser ApplicationUser { get; set; } = default!;
        public DateTime DateOfBirth { get; set; }
        public BloodType? Blood_Type { get; set; }
        public ICollection<Appointment>? Appointments { get; set; } = new HashSet<Appointment>();
        public ICollection<Review>? Reviews { get; set; } = new HashSet<Review>();
        public ICollection<DoctorPatient> DoctorPatients { get; set; } = new HashSet<DoctorPatient>();
        public int GetAge()
        {
            var today = DateTime.UtcNow;
            var age = today.Year - DateOfBirth.Year;
            if (today.DayOfYear < DateOfBirth.DayOfYear)
                age--;
            return age;
        }

    }
}
