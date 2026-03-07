using Vezeeta_Clone.Core.Features.Auth.Commands.Models;
using Vezeeta_Clone.Data.Entities;

namespace Vezeeta_Clone.Core.Mapping.DoctorMapping
{
    public partial class DoctorProfile
    {
        public void DoctorAuthMapping()
        {

            CreateMap<RegisterDoctorCommand, ApplicationUser>();
            CreateMap<RegisterDoctorCommand, Doctor>()
                .ForMember(d => d.ApplicationUser, opt => opt.Ignore());
        }
    }
}
