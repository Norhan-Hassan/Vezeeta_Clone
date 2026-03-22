using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vezeeta_Clone.Data.Entities
{
    public class Review : BaseEntity
    {
        [Range(0.0, 5.0)]
        public double Rating { get; set; }

        [MaxLength(250)]
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public bool IsAnonymous { get; set; }

        [Required]
        [ForeignKey("Patient")]
        public string PatientId { get; set; }
        public Patient? Patient { get; set; }

        [Required]
        [ForeignKey("Doctor")]
        public string DoctorId { get; set; }
        public Doctor? Doctor { get; set; }

    }
}
