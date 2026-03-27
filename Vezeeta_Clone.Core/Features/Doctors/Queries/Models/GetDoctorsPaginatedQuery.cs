using MediatR;
using Vezeeta_Clone.Core.Bases;
using Vezeeta_Clone.Core.Features.Doctors.Queries.Results;
using Vezeeta_Clone.Core.Wrappers;
using Vezeeta_Clone.Data.Entities.Enums;
using Vezeeta_Clone.Data.Helper;

namespace Vezeeta_Clone.Core.Features.Doctors.Queries.Models
{

    public class GetDoctorsPaginatedQuery : PaginationFilter, IRequest<Response<PaginatedResult<GetDoctorsPaginatedQueryResult>>>
    {
        public OrderingCriteria? OrderBy { get; set; }
        public string? Search { get; set; }
        public int? cityId { get; set; }
        public int? regionId { get; set; }
        public int? specializationId { get; set; }
        public Gender? gender { get; set; }
        public Title? Title { get; set; }
    }
}
