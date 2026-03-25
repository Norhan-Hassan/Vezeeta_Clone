using MediatR;
using System.Text.Json.Serialization;
using Vezeeta_Clone.Core.Bases;

namespace Vezeeta_Clone.Core.Features.MedicalRecords.Commands.Models
{
    public class CreateDiagnosisCommand : IRequest<Response<string>>
    {
        [JsonIgnore]
        public int MedicalRecordId { get; set; }
        public string Description { get; set; }
    }
}
