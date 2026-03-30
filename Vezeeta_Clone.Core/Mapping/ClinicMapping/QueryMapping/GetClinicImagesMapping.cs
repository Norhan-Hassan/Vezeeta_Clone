using Vezeeta_Clone.Core.Features.Clinics.Queries.Results;
using Vezeeta_Clone.Data.Entities;

namespace Vezeeta_Clone.Core.Mapping.ClinicMapping
{
    public partial class ClinicMappingProfile
    {
        public void GetClinicImagesMapping()
        {
            CreateMap<ClinicImage, GetClinicImagesQueryResult>()
                .ForMember(des => des.Id, opt => opt.MapFrom(src => src.ID))
                .ForMember(des => des.Url, opt => opt.MapFrom(src => src.Url));
        }
    }
}
