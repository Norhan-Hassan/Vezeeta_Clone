using Vezeeta_Clone.Data.Entities;

namespace Vezeeta_Clone.Service.Abstract
{
    public interface IDoctorService
    {
        Task<Doctor> GetDoctorByIDAsync(string id);
        Task<Doctor> GetDoctorWithClinicByIDAsync(string id);
        IQueryable<Doctor> FilteredDoctorsAsQuerable(int? specializationId, string? search, int? cityId, int? regionId);
        Task<(double Average, int Count)> GetDoctorRatingInfo(string id);
        IQueryable<Review> GetDoctorReviews(string id);
        Task UpdateDoctorAsync(Doctor doctor);
    }
}
