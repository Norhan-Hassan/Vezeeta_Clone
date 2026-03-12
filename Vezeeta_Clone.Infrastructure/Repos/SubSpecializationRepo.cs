using Microsoft.EntityFrameworkCore;
using Vezeeta_Clone.Data.Entities;
using Vezeeta_Clone.Infrastructure.Abstract;
using Vezeeta_Clone.Infrastructure.Context;
using Vezeeta_Clone.Infrastructure.InfrastructureBases;

namespace Vezeeta_Clone.Infrastructure.Repos
{
    public class SubSpecializationRepo : GenericRepositoryAsync<SubSpecialization>, ISubSpecializationRepo
    {
        private readonly DbSet<SubSpecialization> _subSpecialization;

        public SubSpecializationRepo(ApplicationDbContext dbContext) : base(dbContext)
        {
            _subSpecialization = dbContext.Set<SubSpecialization>();
        }
    }
}
