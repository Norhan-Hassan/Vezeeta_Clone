using Vezeeta_Clone.Core.Features.Patients.Queries.Results;
using Vezeeta_Clone.Data.Entities;

namespace Vezeeta_Clone.Core.Mapping.PatientsMapping
{
    public partial class PatientProfile
    {

        public void GetPatientProfileMapping()
        {
            CreateMap<Patient, GetPatientProfileQueryResult>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.AppUserID))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.ApplicationUser.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.ApplicationUser.LastName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.ApplicationUser.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.ApplicationUser.PhoneNumber))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth.ToShortDateString()))
                .ForMember(dest => dest.Blood_Type, opt => opt.MapFrom(src => src.Blood_Type.ToString()));
        }
    }
}
