using System.ComponentModel.DataAnnotations.Schema;

namespace Vezeeta_Clone.Data.Entities
{
    public class Region : BaseEntity
    {
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public string LocalizedName => GetLocalizedName(NameEn, NameAr);////
        [ForeignKey("City")]
        public int CityId { get; set; }
        public City City { get; set; }
    }
}
