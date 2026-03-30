using AutoMapper;

namespace Vezeeta_Clone.Core.Mapping.ClinicMapping
{
    public partial class ClinicMappingProfile : Profile
    {
        public ClinicMappingProfile()
        {
            AddClinicImageMapping();
            RegisterDoctorClinicMapping();
            GetClinicImagesMapping();
        }
    }
}
