using System.ComponentModel.DataAnnotations.Schema;

namespace Vezeeta_Clone.Data.Entities
{
    public class SubSpecialization : BaseEntity
    {
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public string LocalizedName => GetLocalizedName(NameEn, NameAr);////

        [ForeignKey("Specialization")]
        public int SpecializationId { get; set; }
        public Specialization Specialization { get; set; }

        public ICollection<Doctor>? Doctors { get; set; } = new HashSet<Doctor>();
    }
}
