using Microsoft.EntityFrameworkCore;
using Vezeeta_Clone.Data.Entities;
using Vezeeta_Clone.Infrastructure.Abstract;
using Vezeeta_Clone.Infrastructure.Context;
using Vezeeta_Clone.Infrastructure.InfrastructureBases;

namespace Vezeeta_Clone.Infrastructure.Repos
{
    public class DoctorAvailabilityRepo : GenericRepositoryAsync<DoctorAvailability>, IDoctorAvailabilityRepo
    {
        private readonly DbSet<DoctorAvailability> _doctorAvailability;
        public DoctorAvailabilityRepo(ApplicationDbContext dbcontext) : base(dbcontext)
        {
            _doctorAvailability = _dbContext.Set<DoctorAvailability>();
        }
    }
}
