using Microsoft.EntityFrameworkCore;
using Vezeeta_Clone.Data.Entities;
using Vezeeta_Clone.Infrastructure.Abstract;
using Vezeeta_Clone.Infrastructure.Context;
using Vezeeta_Clone.Infrastructure.InfrastructureBases;

namespace Vezeeta_Clone.Infrastructure.Repos
{
    public class ClinicRepo : GenericRepositoryAsync<Clinic>, IClinicRepo
    {
        private DbSet<Clinic> _clinic;
        public ClinicRepo(ApplicationDbContext dbContext) : base(dbContext)
        {
            _clinic = dbContext.Set<Clinic>();
        }
    }
}
