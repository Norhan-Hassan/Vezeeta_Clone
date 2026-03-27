using MediatR;
using Vezeeta_Clone.Core.Bases;
using Vezeeta_Clone.Core.Features.Payments.Queries.Results;

namespace Vezeeta_Clone.Core.Features.Payments.Queries.Models
{
    public class GetPaymentByAppointmentIdQuery : IRequest<Response<GetPaymentInfoQueryResult>>
    {
        public int AppointmentId { get; set; }
    }
}
