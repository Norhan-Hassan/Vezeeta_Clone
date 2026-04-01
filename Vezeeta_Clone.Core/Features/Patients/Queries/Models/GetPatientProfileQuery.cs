using MediatR;
using Vezeeta_Clone.Core.Bases;
using Vezeeta_Clone.Core.Features.Patients.Queries.Results;

namespace Vezeeta_Clone.Core.Features.Patients.Queries.Models
{
    public class GetPatientProfileQuery : IRequest<Response<GetPatientProfileQueryResult>>
    {

    }
}
