using System.ComponentModel.DataAnnotations.Schema;

namespace Vezeeta_Clone.Data.Entities
{
    public class MedicalRecord : BaseEntity
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("DoctorPatient")]
        public int DoctorPatientId { get; set; }
        public DoctorPatient DoctorPatient { get; set; }

        [ForeignKey("Appointment")]
        public int AppointmentId { get; set; }
        public Appointment Appointment { get; set; }

        public ICollection<Diagnosis> Diagnoses { get; set; } = new HashSet<Diagnosis>();
        public ICollection<EPrescription> EPrescriptions { get; set; } = new HashSet<EPrescription>();
    }
}
