using MediatR;
using Vezeeta_Clone.Core.Bases;
using Vezeeta_Clone.Core.Features.Doctors.Queries.Results;

namespace Vezeeta_Clone.Core.Features.Doctors.Queries.Models
{
    public class GetDoctorExaminationDetailsQuery : IRequest<Response<GetDoctorExaminationDetailsQueryResult>>
    {
        public string Id { get; set; }
    }
}
