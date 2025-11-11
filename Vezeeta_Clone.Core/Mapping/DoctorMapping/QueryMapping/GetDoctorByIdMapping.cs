using Vezeeta_Clone.Core.Features.Doctor.Queries.Results;
using Vezeeta_Clone.Data.Entities;

namespace Vezeeta_Clone.Core.Mapping.DoctorMapping
{
    public partial class DoctorProfile
    {
        public void GetDoctorByIdMapping()
        {
            CreateMap<Doctor, GetDoctorByIdResponse>()
                .ForMember(dest => dest.Specialization, opt => opt.MapFrom(src => src.Specialization.Name ?? "Not Assigned to Specialization yet"));
        }
    }
}
