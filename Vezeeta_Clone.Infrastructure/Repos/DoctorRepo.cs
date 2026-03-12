using Microsoft.EntityFrameworkCore;
using Vezeeta_Clone.Data.Entities;
using Vezeeta_Clone.Infrastructure.Abstract;
using Vezeeta_Clone.Infrastructure.Context;
using Vezeeta_Clone.Infrastructure.InfrastructureBases;

namespace Vezeeta_Clone.Infrastructure.Repos
{
    public class DoctorRepo : GenericRepositoryAsync<Doctor>, IDoctorRepo
    {
        private readonly DbSet<Doctor> _doctor;
        public DoctorRepo(ApplicationDbContext dbContext) : base(dbContext)
        {
            _doctor = _dbContext.Set<Doctor>();
        }

        public IQueryable<Doctor> GetAllDoctorsWithIncludesAsQuerable()
        {
            var doctors = base.GetTableNoTracking()
                                     .Include(d => d.Specialization)
                                     .Include(d => d.ApplicationUser)
                                     .Include(d => d.Clinic).ThenInclude(c => c.Region).AsQueryable();
            return doctors;
        }
    }
}
