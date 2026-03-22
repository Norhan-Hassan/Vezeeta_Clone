using MediatR;
using System.Text.Json.Serialization;
using Vezeeta_Clone.Core.Bases;

namespace Vezeeta_Clone.Core.Features.Reviews.Commands.Models
{
    public class UpdateReviewCommand : IRequest<Response<string>>
    {
        [JsonIgnore]
        public int Id { get; set; }
        public double Rating { get; set; }
        public string Comment { get; set; }
    }
}
