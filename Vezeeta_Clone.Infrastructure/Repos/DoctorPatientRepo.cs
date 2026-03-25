using Microsoft.EntityFrameworkCore;
using Vezeeta_Clone.Data.Entities;
using Vezeeta_Clone.Infrastructure.Abstract;
using Vezeeta_Clone.Infrastructure.Context;
using Vezeeta_Clone.Infrastructure.InfrastructureBases;

namespace Vezeeta_Clone.Infrastructure.Repos
{
    public class DoctorPatientRepo : GenericRepositoryAsync<DoctorPatient>, IDoctorPatientRepo
    {
        private readonly DbSet<DoctorPatient> _doctorpatient;
        public DoctorPatientRepo(ApplicationDbContext dbContext) : base(dbContext)
        {
            _doctorpatient = _dbContext.Set<DoctorPatient>();
        }


    }
}
