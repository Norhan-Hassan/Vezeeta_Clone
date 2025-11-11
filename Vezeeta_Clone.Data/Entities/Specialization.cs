namespace Vezeeta_Clone.Data.Entities
{
    public class Specialization : BaseEntity
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public ICollection<Doctor>? Doctors { get; set; } = new HashSet<Doctor>();
    }
}
