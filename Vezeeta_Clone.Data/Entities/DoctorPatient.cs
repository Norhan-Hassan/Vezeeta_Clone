using System.ComponentModel.DataAnnotations.Schema;

namespace Vezeeta_Clone.Data.Entities
{
    public class DoctorPatient : BaseEntity
    {
        [ForeignKey(nameof(Doctor))]
        public string DoctorId { get; set; }
        public Doctor Doctor { get; set; }

        [ForeignKey(nameof(Patient))]
        public string PatientId { get; set; }
        public Patient Patient { get; set; }
        public int TotalVisits { get; set; }
        public DateTime FirstVisitAt { get; set; }
        public DateTime LastVisitAt { get; set; } = DateTime.UtcNow;
        public ICollection<MedicalRecord> MedicalRecords { get; set; }

    }
}
