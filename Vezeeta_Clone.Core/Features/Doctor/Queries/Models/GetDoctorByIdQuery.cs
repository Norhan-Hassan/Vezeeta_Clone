using MediatR;
using Vezeeta_Clone.Core.Bases;
using Vezeeta_Clone.Core.Features.Doctor.Queries.Results;

namespace Vezeeta_Clone.Core.Features.Doctor.Queries.Models
{
    public class GetDoctorByIdQuery : IRequest<Response<GetDoctorByIdResponse>>
    {
        public string Id { get; set; }
    }
}
