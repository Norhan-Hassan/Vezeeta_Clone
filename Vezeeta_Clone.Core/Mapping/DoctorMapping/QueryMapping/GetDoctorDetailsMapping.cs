using Microsoft.OpenApi.Extensions;
using Vezeeta_Clone.Core.Features.Doctors.Queries.Results;
using Vezeeta_Clone.Data.Entities;

namespace Vezeeta_Clone.Core.Mapping.DoctorMapping
{
    public partial class DoctorProfile
    {
        public void GetDoctorDetailsMapping()
        {
            CreateMap<Doctor, GetDoctorDetailsQueryResult>()
                .ForMember(dest => dest.Specialization, opt => opt.MapFrom(src => src.Specialization!.LocalizedName))
                .ForMember(dest => dest.SubSpecializations, opt => opt.MapFrom(src => src.SubSpecializations!.Select(s => s.LocalizedName).ToArray()))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title.GetDisplayName()))
                .ForMember(dest => dest.University, opt => opt.MapFrom(src => src.University.LocalizedName))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => string.Concat(src.ApplicationUser.FirstName, " ", src.ApplicationUser.LastName)))
                .ForMember(dest => dest.Picture, opt => opt.MapFrom(src => src.Picture))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));

        }
    }
}
