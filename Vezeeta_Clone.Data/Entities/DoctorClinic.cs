using System.ComponentModel.DataAnnotations.Schema;

namespace Vezeeta_Clone.Data.Entities
{
    public class DoctorClinic : BaseEntity
    {
        [ForeignKey("Doctor")]
        public string DoctorId { get; set; }
        public Doctor Doctor { get; set; }

        [ForeignKey("Clinic")]
        public int ClinicId { get; set; }
        public Clinic Clinic { get; set; }
        public decimal? Price { get; set; }
    }
}
