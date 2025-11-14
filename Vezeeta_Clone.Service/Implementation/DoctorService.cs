using Microsoft.EntityFrameworkCore;
using Vezeeta_Clone.Data.Entities;
using Vezeeta_Clone.Infrastructure.Abstract;
using Vezeeta_Clone.Service.Abstract;

namespace Vezeeta_Clone.Service.Implementation
{
    public class DoctorService : IDoctorService
    {
        #region Fields
        private readonly IDoctorRepo _doctorRepo;
        #endregion


        #region Constructors
        public DoctorService(IDoctorRepo doctorRepo)
        {
            _doctorRepo = doctorRepo;
        }
        #endregion

        #region Methods
        public async Task<Doctor> GetDoctorByIDAsync(string id)
        {
            var doctor = await _doctorRepo.GetTableNoTracking()
                                     .Include(d => d.Specialization)
                                     .FirstOrDefaultAsync(d => d.AppUserID == id);
            return doctor;
        }
        #endregion
    }
}
