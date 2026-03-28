using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vezeeta_Clone.Data.Entities
{
    public class Picture
    {
        [Key]
        public int ID { get; set; }
        public string PublicId { get; set; }
        public string PhotoUrl { get; set; }


        [ForeignKey(nameof(Doctor))]
        public string? DoctorId { get; set; }
        public Doctor? Doctor { get; set; }

        [ForeignKey(nameof(Clinic))]
        public string? ClinicId { get; set; }
        public Clinic? Clinic { get; set; }
    }
}
