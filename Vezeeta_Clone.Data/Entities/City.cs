namespace Vezeeta_Clone.Data.Entities
{
    public class City : BaseEntity
    {
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public string LocalizedName => GetLocalizedName(NameEn, NameAr);////
        public virtual ICollection<Region>? Regions { get; set; } = new HashSet<Region>();
    }
}
