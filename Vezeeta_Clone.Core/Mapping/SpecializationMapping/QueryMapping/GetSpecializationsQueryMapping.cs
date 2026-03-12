using Vezeeta_Clone.Core.Features.Specializations.Queries.Results;
using Vezeeta_Clone.Data.Entities;

namespace Vezeeta_Clone.Core.Mapping.SpecializationMapping
{
    public partial class SpecializationProfile
    {
        public void GetSpecializationsQueryMapping()
        {
            CreateMap<Specialization, GetSpecializationsQueryResult>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.LocalizedName))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ID));
        }
    }
}
