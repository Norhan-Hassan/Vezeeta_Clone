using MediatR;
using Vezeeta_Clone.Core.Bases;
using Vezeeta_Clone.Core.Features.Doctors.Queries.Results;
using Vezeeta_Clone.Core.Wrappers;
using Vezeeta_Clone.Data.Entities.Enums;

namespace Vezeeta_Clone.Core.Features.Doctors.Queries.Models
{
    public class GetDoctorAppointmentsQuery : PaginationFilter, IRequest<Response<PaginatedResult<GetDoctorAppointmentsQueryResult>>>
    {
        public AppointmentStatus? status { get; set; }
        public AvailabilityMethod availabilityMethod { get; set; } = AvailabilityMethod.Offline;
        public DateOnly? FromDate { get; set; }
        public DateOnly? ToDate { get; set; }
    }
}
