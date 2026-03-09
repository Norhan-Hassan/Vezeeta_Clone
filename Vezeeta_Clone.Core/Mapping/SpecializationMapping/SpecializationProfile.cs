using AutoMapper;

namespace Vezeeta_Clone.Core.Mapping.SpecializationMapping
{
    public partial class SpecializationProfile : Profile
    {
        public SpecializationProfile()
        {
            CreateSpecializationMapping();
            UpdateSpecializationMapping();
            GetSupSpecializationBySpecID();
        }
    }
}
