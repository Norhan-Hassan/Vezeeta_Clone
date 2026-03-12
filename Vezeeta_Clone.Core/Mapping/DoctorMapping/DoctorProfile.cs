using AutoMapper;

namespace Vezeeta_Clone.Core.Mapping.DoctorMapping
{
    public partial class DoctorProfile : Profile
    {
        public DoctorProfile()
        {
            DoctorAuthMapping();
            GetDoctorDetailsMapping();
            GetDoctorExaminationDetailsQueryMapping();
        }
    }
}
