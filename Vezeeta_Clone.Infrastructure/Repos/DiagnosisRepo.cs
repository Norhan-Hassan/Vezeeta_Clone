using Microsoft.EntityFrameworkCore;
using Vezeeta_Clone.Data.Entities;
using Vezeeta_Clone.Infrastructure.Abstract;
using Vezeeta_Clone.Infrastructure.Context;
using Vezeeta_Clone.Infrastructure.InfrastructureBases;

namespace Vezeeta_Clone.Infrastructure.Repos
{
    public class DiagnosisRepo : GenericRepositoryAsync<Diagnosis>, IDiagnosisRepo
    {
        private readonly DbSet<Diagnosis> _diagnosis;
        public DiagnosisRepo(ApplicationDbContext dbContext) : base(dbContext)
        {
            _diagnosis = _dbContext.Set<Diagnosis>();
        }
    }
}
