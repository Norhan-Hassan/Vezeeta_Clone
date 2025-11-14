using System.ComponentModel.DataAnnotations.Schema;

namespace Vezeeta_Clone.Data.Entities
{
    public class MedicalRecord : BaseEntity
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("Patient")]
        public string PatientId { get; set; }
        public Patient Patient { get; set; }

        /// <summary>
        /// not every medical record is linked to an appointment
        /// </summary>

        [ForeignKey("Appointment")]
        public int? AppointmentId { get; set; }
        public Appointment? Appointment { get; set; }

        [ForeignKey("Doctor")]
        public string DoctorId { get; set; }
        public Doctor Doctor { get; set; }

        public ICollection<Diagnosis> Diagnoses { get; set; } = new HashSet<Diagnosis>();
        public ICollection<EPrescription> EPrescriptions { get; set; } = new HashSet<EPrescription>();
    }
}
