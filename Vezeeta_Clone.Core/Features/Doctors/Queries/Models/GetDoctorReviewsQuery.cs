using MediatR;
using Microsoft.AspNetCore.Mvc;
using Vezeeta_Clone.Core.Bases;
using Vezeeta_Clone.Core.Features.Doctors.Queries.Results;
using Vezeeta_Clone.Core.Wrappers;

namespace Vezeeta_Clone.Core.Features.Doctors.Queries.Models
{
    public class GetDoctorReviewsQuery : IRequest<Response<PaginatedResult<GetDoctorReviewsQueryResult>>>
    {
        [FromRoute(Name = "Id")]
        public string DoctorId { get; set; }
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }

    }
}
