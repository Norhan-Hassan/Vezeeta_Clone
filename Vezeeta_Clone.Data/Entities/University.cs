namespace Vezeeta_Clone.Data.Entities
{
    public class University : BaseEntity
    {
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public string LocalizedName => GetLocalizedName(NameEn, NameAr);////

        public ICollection<Doctor> Doctors { get; set; } = new HashSet<Doctor>();
    }
}
