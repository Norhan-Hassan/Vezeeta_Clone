using Microsoft.EntityFrameworkCore;
using Vezeeta_Clone.Data.Entities;
using Vezeeta_Clone.Infrastructure.Abstract;
using Vezeeta_Clone.Infrastructure.Context;
using Vezeeta_Clone.Infrastructure.InfrastructureBases;

namespace Vezeeta_Clone.Infrastructure.Repos
{
    public class PrescriptionRepo : GenericRepositoryAsync<EPrescription>, IPrescriptionRepo
    {
        private readonly DbSet<EPrescription> _prescription;
        public PrescriptionRepo(ApplicationDbContext dbContext) : base(dbContext)
        {
            _prescription = _dbContext.Set<EPrescription>();
        }
    }
}
