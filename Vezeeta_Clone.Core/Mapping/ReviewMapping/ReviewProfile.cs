using AutoMapper;

namespace Vezeeta_Clone.Core.Mapping.ReviewMapping
{
    public partial class ReviewProfile : Profile
    {
        public ReviewProfile()
        {
            MakeReviewMapping();
            UpdateReviewMapping();
        }
    }
}
