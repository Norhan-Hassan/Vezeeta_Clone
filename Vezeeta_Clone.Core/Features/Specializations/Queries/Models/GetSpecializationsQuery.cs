using MediatR;
using Vezeeta_Clone.Core.Bases;
using Vezeeta_Clone.Core.Features.Specializations.Queries.Results;

namespace Vezeeta_Clone.Core.Features.Specializations.Queries.Models
{
    public class GetSpecializationsQuery : IRequest<Response<List<GetSpecializationsQueryResult>>>
    {

    }
}
