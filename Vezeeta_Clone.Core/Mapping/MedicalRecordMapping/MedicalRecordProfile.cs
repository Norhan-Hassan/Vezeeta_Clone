using AutoMapper;

namespace Vezeeta_Clone.Core.Mapping.MedicalRecordMapping
{
    public partial class MedicalRecordProfile : Profile
    {
        public MedicalRecordProfile()
        {
            CreateMedicalRecordMapping();
            CreateDiagnosisMapping();
            CreateEPrescriptionMapping();
        }
    }
}
