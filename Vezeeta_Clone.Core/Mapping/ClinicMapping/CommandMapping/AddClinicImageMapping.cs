using Vezeeta_Clone.Data.Entities;

namespace Vezeeta_Clone.Core.Mapping.ClinicMapping
{
    public partial class ClinicMappingProfile
    {
        public void AddClinicImageMapping()
        {
            CreateMap<string, ClinicImage>()
                .ForMember(dest => dest.Url, opt => opt.MapFrom(src => src));
        }
    }
}
