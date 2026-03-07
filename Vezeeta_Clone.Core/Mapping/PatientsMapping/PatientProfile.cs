using AutoMapper;

namespace Vezeeta_Clone.Core.Mapping.PatientsMapping
{
    public partial class PatientProfile : Profile
    {
        public PatientProfile()
        {
            PatientAuthMapping();

        }
    }
}
