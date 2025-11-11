namespace Vezeeta_Clone.Data.Entities
{
    public class City : BaseEntity
    {
        public string Name { get; set; }
        public virtual ICollection<Region>? Regions { get; set; } = new HashSet<Region>();
    }
}
