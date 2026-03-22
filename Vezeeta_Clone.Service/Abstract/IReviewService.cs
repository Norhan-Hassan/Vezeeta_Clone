using Vezeeta_Clone.Data.Entities;

namespace Vezeeta_Clone.Service.Abstract
{
    public interface IReviewService
    {
        Task<bool> MakeReviewAsync(Review review, string patientId);
        Task<bool> UpdateReviewAsync(Review review, string patientId);
        Task DeleteReviewAsync(Review review, string patientId);
        Task<Review> GetReviewById(int reviewId);
    }
}
