using MediatR;
using Vezeeta_Clone.Core.Bases;
using Vezeeta_Clone.Core.Features.MedicalRecords.Queries.Results;

namespace Vezeeta_Clone.Core.Features.MedicalRecords.Queries.Models
{
    public class GetMedicalReportQuery : IRequest<Response<GetMedicalReportQueryResult>>
    {
        public int MedicalRecordId { get; set; }
    }
}
