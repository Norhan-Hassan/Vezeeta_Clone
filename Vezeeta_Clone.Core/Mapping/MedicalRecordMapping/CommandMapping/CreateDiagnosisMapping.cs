using Vezeeta_Clone.Core.Features.MedicalRecords.Commands.Models;
using Vezeeta_Clone.Data.Entities;

namespace Vezeeta_Clone.Core.Mapping.MedicalRecordMapping
{
    public partial class MedicalRecordProfile
    {
        public void CreateDiagnosisMapping()
        {
            CreateMap<CreateDiagnosisCommand, Diagnosis>();
        }
    }
}
