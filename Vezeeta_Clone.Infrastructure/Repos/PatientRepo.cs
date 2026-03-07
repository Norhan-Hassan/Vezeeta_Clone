using Microsoft.EntityFrameworkCore;
using Vezeeta_Clone.Data.Entities;
using Vezeeta_Clone.Infrastructure.Abstract;
using Vezeeta_Clone.Infrastructure.Context;
using Vezeeta_Clone.Infrastructure.InfrastructureBases;

namespace Vezeeta_Clone.Infrastructure.Repos
{
    public class PatientRepo : GenericRepositoryAsync<Patient>, IPatientRepo
    {
        private readonly DbSet<Patient> _patient;
        public PatientRepo(ApplicationDbContext dbContext) : base(dbContext)
        {
            _patient = _dbContext.Set<Patient>();
        }
    }
}
