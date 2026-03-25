using MediatR;
using Vezeeta_Clone.Core.Bases;

namespace Vezeeta_Clone.Core.Features.Reviews.Commands.Models
{
    public class MakeReviewCommand : IRequest<Response<string>>
    {
        public string DoctorId { get; set; }
        public double Rating { get; set; } = 0;
        public string? Comment { get; set; }
        public bool IsAnonymous { get; set; } = false;
    }
}
