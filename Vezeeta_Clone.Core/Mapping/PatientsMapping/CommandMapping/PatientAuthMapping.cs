using Vezeeta_Clone.Core.Features.Auth.Commands.Models;
using Vezeeta_Clone.Data.Entities;

namespace Vezeeta_Clone.Core.Mapping.PatientsMapping
{
    public partial class PatientProfile
    {
        public void PatientAuthMapping()
        {
            CreateMap<RegisterPatientCommand, Patient>()
                .ForMember(dest => dest.ApplicationUser, opt => opt.Ignore());

            CreateMap<RegisterPatientCommand, ApplicationUser>();



        }
    }
}
