using Vezeeta_Clone.Core.Features.Specializations.Commands.Models;
using Vezeeta_Clone.Data.Entities;

namespace Vezeeta_Clone.Core.Mapping.SpecializationMapping
{
    public partial class SpecializationProfile
    {
        public void UpdateSpecializationMapping()
        {
            CreateMap<UpdateSpecializationCommand, Specialization>()
                .ForMember(dest => dest.ID, opt => opt.Ignore());
        }
    }
}
