using Vezeeta_Clone.Core.Features.Reviews.Commands.Models;
using Vezeeta_Clone.Data.Entities;

namespace Vezeeta_Clone.Core.Mapping.ReviewMapping
{
    public partial class ReviewProfile
    {
        public void MakeReviewMapping()
        {
            CreateMap<MakeReviewCommand, Review>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
        }
    }
}
