using MediatR;
using Vezeeta_Clone.Core.Bases;
using Vezeeta_Clone.Core.Features.Appointments.Queries.Results;

namespace Vezeeta_Clone.Core.Features.Appointments.Queries.Models
{
    public class GetAppointmentDetailesQuery : IRequest<Response<GetAppointmentDetailsQueryResult>>
    {
        public int Id { get; set; }
    }
}
