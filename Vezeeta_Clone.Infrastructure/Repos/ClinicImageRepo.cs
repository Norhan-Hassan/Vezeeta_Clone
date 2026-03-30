using Microsoft.EntityFrameworkCore;
using Vezeeta_Clone.Data.Entities;
using Vezeeta_Clone.Infrastructure.Abstract;
using Vezeeta_Clone.Infrastructure.Context;
using Vezeeta_Clone.Infrastructure.InfrastructureBases;

namespace Vezeeta_Clone.Infrastructure.Repos
{
    public class ClinicImageRepo : GenericRepositoryAsync<ClinicImage>, IClinicImageRepo
    {
        private DbSet<ClinicImage> _clinicImages;
        public ClinicImageRepo(ApplicationDbContext dbContext) : base(dbContext)
        {
            _clinicImages = dbContext.Set<ClinicImage>();
        }
    }
}
