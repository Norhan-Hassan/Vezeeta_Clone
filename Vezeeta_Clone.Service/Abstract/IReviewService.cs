using Vezeeta_Clone.Data.Entities;

namespace Vezeeta_Clone.Service.Abstract
{
    public interface IReviewService
    {
        Task<bool> MakeReviewAsync(Review review, string patientId);
    }
}
