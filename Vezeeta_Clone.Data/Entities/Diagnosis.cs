using System.ComponentModel.DataAnnotations.Schema;

namespace Vezeeta_Clone.Data.Entities
{
    public class Diagnosis : BaseEntity
    {
        public string Description { get; set; }
        public DateTime DiagnosedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("MedicalRecord")]
        public int MedicalRecordId { get; set; }
        public MedicalRecord MedicalRecord { get; set; }

        [ForeignKey("Doctor")]
        public string DoctorId { get; set; }
        public Doctor Doctor { get; set; }

    }
}
