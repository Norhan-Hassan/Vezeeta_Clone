using MediatR;
using Vezeeta_Clone.Core.Bases;

namespace Vezeeta_Clone.Core.Features.Reviews.Commands.Models
{
    public class DeleteReviewCommand : IRequest<Response<string>>
    {
        public int Id { get; set; }
    }
}
