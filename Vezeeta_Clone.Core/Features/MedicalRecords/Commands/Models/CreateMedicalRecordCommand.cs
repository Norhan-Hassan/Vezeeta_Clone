using MediatR;
using Vezeeta_Clone.Core.Bases;

namespace Vezeeta_Clone.Core.Features.MedicalRecords.Commands.Models
{
    public class CreateMedicalRecordCommand : IRequest<Response<string>>
    {
        public string PatientId { get; set; }
        public int AppointmentId { get; set; }
    }
}
