using System.ComponentModel.DataAnnotations.Schema;

namespace Vezeeta_Clone.Data.Entities
{
    public class ClinicImage : BaseEntity
    {
        public string Url { get; set; }

        [ForeignKey(nameof(Clinic))]
        public int clinicId { get; set; }
        public Clinic Clinic { get; set; }
    }
}
