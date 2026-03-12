using Vezeeta_Clone.Core.Features.Clinics.Commands.Models;
using Vezeeta_Clone.Core.Features.Clinics.Shared;
using Vezeeta_Clone.Data.Entities;

namespace Vezeeta_Clone.Core.Mapping.ClinicMapping
{
    public partial class ClinicMappingProfile
    {
        public void RegisterDoctorClinicMapping()
        {
            CreateMap<LocationDto, Location>();

            CreateMap<RegisterClinicForDoctorCommand, Clinic>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ClinicName))
                .ForMember(dest => dest.ClinicLocation, opt => opt.MapFrom(src => src.ClinicLocation));

        }
    }
}
