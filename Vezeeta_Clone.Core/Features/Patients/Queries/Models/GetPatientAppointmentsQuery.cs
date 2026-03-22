using MediatR;
using Vezeeta_Clone.Core.Bases;
using Vezeeta_Clone.Core.Features.Patients.Queries.Results;
using Vezeeta_Clone.Core.Wrappers;
using Vezeeta_Clone.Data.Entities.Enums;

namespace Vezeeta_Clone.Core.Features.Patients.Queries.Models
{
    public class GetPatientAppointmentsQuery : PaginationFilter, IRequest<Response<PaginatedResult<GetPatientAppointmentsQueryResult>>>
    {
        public AppointmentStatus? Status { get; set; }
    }
}
