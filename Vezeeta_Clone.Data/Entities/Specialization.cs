namespace Vezeeta_Clone.Data.Entities
{
    public class Specialization : BaseEntity
    {
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public ICollection<SubSpecialization> SubSpecializations { get; set; } = new HashSet<SubSpecialization>();
        public ICollection<Doctor>? Doctors { get; set; } = new HashSet<Doctor>();
    }
}
