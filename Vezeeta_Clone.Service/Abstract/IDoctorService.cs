using Vezeeta_Clone.Data.Entities;
using Vezeeta_Clone.Data.Entities.Enums;
using Vezeeta_Clone.Data.Helper;

namespace Vezeeta_Clone.Service.Abstract
{
    public interface IDoctorService
    {
        Task<Doctor> GetDoctorByIDAsync(string id);
        Task<Doctor> GetDoctorWithClinicByIDAsync(string id);
        Task<Doctor?> GetDoctorByIdWithoutIncludesAsync(string id);
        IQueryable<Doctor> FilteredDoctorsAsQuerable(int? specializationId, string? search, int? cityId, int? regionId, OrderingCriteria? orderBy, Title? title, Gender? gender);
        Task<(double Average, int Count)> GetDoctorRatingInfo(string id);
        IQueryable<Review> GetDoctorReviews(string id);
        Task UpdateDoctorAsync(Doctor doctor);

        Task<bool> CompleteDoctorInfoAsync(Doctor doctor, int[]? subSpecIds, string description);
    }
}
