using Microsoft.EntityFrameworkCore;
using Vezeeta_Clone.Data.Entities;
using Vezeeta_Clone.Infrastructure.Abstract;
using Vezeeta_Clone.Infrastructure.Context;
using Vezeeta_Clone.Infrastructure.InfrastructureBases;

namespace Vezeeta_Clone.Infrastructure.Repos
{
    public class SpecializationRepo : GenericRepositoryAsync<Specialization>, ISpecializationRepo
    {
        private readonly DbSet<Specialization> _specialization;
        public SpecializationRepo(ApplicationDbContext dbContext) : base(dbContext)
        {
            _specialization = dbContext.Set<Specialization>();
        }
    }
}
