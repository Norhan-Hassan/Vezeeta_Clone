using Vezeeta_Clone.Core.Features.Doctors.Queries.Results;
using Vezeeta_Clone.Data.Entities;

namespace Vezeeta_Clone.Core.Mapping.DoctorMapping
{
    public partial class DoctorProfile
    {
        public void GetDoctorExaminationDetailsQueryMapping()
        {
            CreateMap<Doctor, GetDoctorExaminationDetailsQueryResult>()
                 .ForMember(dest => dest.ClinicAddress, opt => opt.MapFrom(src => src.Clinic!.Address))
                 .ForMember(dest => dest.ClinicCity, opt => opt.MapFrom(src => src.Clinic!.Region.City.LocalizedName))
                 .ForMember(dest => dest.ClinicRegion, opt => opt.MapFrom(src => src.Clinic!.Region.LocalizedName))
                 .ForMember(dest => dest.WaitingTimeInMinutes, opt => opt.MapFrom(src => src.Clinic!.WaitingTimeInMinutes))
                 .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Clinic!.Price));
        }
    }
}
