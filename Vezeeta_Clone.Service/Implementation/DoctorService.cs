using Microsoft.EntityFrameworkCore;
using Vezeeta_Clone.Data.Entities;
using Vezeeta_Clone.Data.Helper;
using Vezeeta_Clone.Infrastructure.InfrastructureBases;
using Vezeeta_Clone.Service.Abstract;

namespace Vezeeta_Clone.Service.Implementation
{
    public class DoctorService : IDoctorService
    {
        #region Fields
        private readonly IUnitOfWork _unitOfWork;
        #endregion


        #region Constructors
        public DoctorService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        #endregion

        #region Methods
        public async Task<Doctor?> GetDoctorByIDAsync(string id)
        {
            var doctor = await _unitOfWork._doctorRepo.GetTableNoTracking()
                                     .Include(d => d.Specialization)
                                     .Include(s => s!.SubSpecializations)
                                     .Include(d => d.ApplicationUser)
                                     .Include(d => d.University)
                                     .FirstOrDefaultAsync(d => d.AppUserID == id);
            return doctor;
        }

        public async Task<Doctor?> GetDoctorByIdWithoutIncludesAsync(string id)
        {
            var doctor = await _unitOfWork._doctorRepo.GetTableNoTracking()
                                    .Include(d => d.ApplicationUser)
                                     .FirstOrDefaultAsync(d => d.AppUserID == id);
            return doctor;
        }
        public async Task<Doctor?> GetDoctorWithClinicByIDAsync(string id)
        {
            var doctor = await _unitOfWork._doctorRepo.GetTableNoTracking()
                                     .Include(d => d.ApplicationUser)
                                     .Include(d => d.Clinic).ThenInclude(c => c.Region).ThenInclude(r => r.City)
                                     .FirstOrDefaultAsync(d => d.AppUserID == id);
            return doctor;
        }


        public IQueryable<Doctor> FilteredDoctorsAsQuerable(int? specializationId, string? search, int? cityId, int? regionId, OrderingCriteria? orderBy)
        {
            var doctors = _unitOfWork._doctorRepo.GetAllDoctorsWithIncludesAsQuerable().Where(d => d.Clinic != null && d.Clinic.DoctorId != null);

            var filteredDoctors = doctors;

            if (specializationId.HasValue)
            {
                filteredDoctors = filteredDoctors.Where(d => d.SpecializationId == specializationId);
            }
            if (cityId.HasValue)
            {
                filteredDoctors = filteredDoctors.Where(d => d.Clinic!.Region.CityId == cityId);
                if (regionId.HasValue)
                {
                    filteredDoctors = filteredDoctors.Where(d => d.Clinic!.RegionId == regionId);
                }
            }

            if (!string.IsNullOrEmpty(search))
            {
                filteredDoctors = filteredDoctors.Where(d => d.ApplicationUser.FirstName.Contains(search) ||
                                                        d.ApplicationUser.LastName.Contains(search) ||
                                                        d.Clinic!.Name.Contains(search));
            }
            if (orderBy.HasValue)
            {
                filteredDoctors = orderBy switch
                {
                    OrderingCriteria.topRated => filteredDoctors.OrderByDescending(d => d.Reviews!.Average(r => r.Rating)),
                    OrderingCriteria.priceLowToHigh => filteredDoctors.OrderBy(d => d.Clinic!.Price),
                    OrderingCriteria.priceHighToLow => filteredDoctors.OrderByDescending(d => d.Clinic!.Price),
                    OrderingCriteria.lessWaitingTime => filteredDoctors.OrderBy(d => d.Clinic!.WaitingTimeInMinutes),
                    _ => filteredDoctors
                };
            }
            return filteredDoctors;
        }


        public async Task<(double Average, int Count)> GetDoctorRatingInfo(string id)
        {
            var result = await _unitOfWork._doctorRepo.GetTableNoTracking().Include(d => d.Reviews)
                .Where(d => d.AppUserID == id)
                .Select(d => new
                {
                    Average = d.Reviews!.Any() ? d.Reviews!.Average(r => r.Rating) : 0,
                    Count = d.Reviews!.Any() ? d.Reviews!.Count() : 0
                }).FirstOrDefaultAsync();

            return (result!.Average, result!.Count);
        }

        public IQueryable<Review> GetDoctorReviews(string id)
        {
            var reviews = _unitOfWork._doctorRepo.GetTableNoTracking()
                                            .Include(d => d.Reviews)
                                            .Where(d => d.AppUserID == id)
                                            .SelectMany(d => d.Reviews!)
                                            .Include(r => r.Patient);

            return reviews;
        }
        public async Task UpdateDoctorAsync(Doctor doctor)
        {
            try
            {
                await _unitOfWork._doctorRepo.UpdateAsync(doctor);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework like Serilog, NLog, etc.)
                Console.WriteLine($"An error occurred while updating the doctor: {ex.Message}");
                throw; // Rethrow the exception to be handled by the caller
            }
        }

        public async Task<bool> CompleteDoctorInfoAsync(Doctor doctor, int[]? subSpecIds, string description)
        {
            if (doctor.IsProfileComplete == false)
            {

                if (subSpecIds != null && subSpecIds.Any())
                {
                    var subspecs = await _unitOfWork._subSpecializationRepo.GetTableNoTracking()
                                            .Where(s => subSpecIds.Contains(s.ID) && s.SpecializationId == doctor.SpecializationId)
                                            .ToListAsync();

                    doctor.SubSpecializations = subspecs;
                }
                doctor.Description = description;
                doctor.IsProfileComplete = true;
                await _unitOfWork._doctorRepo.UpdateAsync(doctor);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            return false;
        }



        #endregion
    }
}
//public async Task<double> GetDoctorAverageRatingNumber(string id)
//{
//    var doctorRatingNumber = await _doctorRepo.GetTableNoTracking()
//                             .Include(d => d.Reviews)
//                             .Where(d => d.AppUserID == id)
//                             .Select(d => d.Reviews.Any()
//                                          ? d.Reviews.Average(r => r.Rating)
//                                          : 0).FirstOrDefaultAsync();

//    return doctorRatingNumber;
//}

//public async Task<int> GetDoctorRatingCount(string id)
//{
//    var doctorRatingCount = await _doctorRepo.GetTableNoTracking()
//                                  .Include(d => d.Reviews)
//                                  .Where(d => d.AppUserID == id)
//                                  .Select(d => d.Reviews.Count()).FirstOrDefaultAsync();
//    return doctorRatingCount;
//}