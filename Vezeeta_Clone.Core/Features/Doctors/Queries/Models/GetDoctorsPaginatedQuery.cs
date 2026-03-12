using MediatR;
using Vezeeta_Clone.Core.Bases;
using Vezeeta_Clone.Core.Features.Doctors.Queries.Results;
using Vezeeta_Clone.Core.Wrappers;

namespace Vezeeta_Clone.Core.Features.Doctors.Queries.Models
{

    public class GetDoctorsPaginatedQuery : IRequest<Response<PaginatedResult<GetDoctorsPaginatedQueryResult>>>
    {
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
        public string[]? OrderBy { get; set; }
        public string? Search { get; set; }
        public int? cityId { get; set; }
        public int? regionId { get; set; }
        public int? specializationId { get; set; }
    }
}
