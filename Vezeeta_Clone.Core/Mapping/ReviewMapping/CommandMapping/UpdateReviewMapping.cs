using Vezeeta_Clone.Core.Features.Reviews.Commands.Models;
using Vezeeta_Clone.Data.Entities;

namespace Vezeeta_Clone.Core.Mapping.ReviewMapping
{
    public partial class ReviewProfile
    {
        public void UpdateReviewMapping()
        {
            CreateMap<UpdateReviewCommand, Review>()
                .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.Id))
                 .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
        }
    }
}
