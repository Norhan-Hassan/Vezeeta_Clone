using MediatR;
using System.Text.Json.Serialization;
using Vezeeta_Clone.Core.Bases;

namespace Vezeeta_Clone.Core.Features.MedicalRecords.Commands.Models
{
    public class CreateEPrescriptionCommand : IRequest<Response<string>>
    {
        [JsonIgnore]
        public int MedicalRecordId { get; set; }
        public List<PrescriptionDto> prescriptions { get; set; }
        public string? Notes { get; set; }

    }
    public class PrescriptionDto
    {
        public string? Medication { get; set; }
        public string? Dose { get; set; }
    }
}
