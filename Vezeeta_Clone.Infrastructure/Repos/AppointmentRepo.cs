using Microsoft.EntityFrameworkCore;
using Vezeeta_Clone.Data.Entities;
using Vezeeta_Clone.Infrastructure.Abstract;
using Vezeeta_Clone.Infrastructure.Context;
using Vezeeta_Clone.Infrastructure.InfrastructureBases;

namespace Vezeeta_Clone.Infrastructure.Repos
{
    public class AppointmentRepo : GenericRepositoryAsync<Appointment>, IAppointmentRepo
    {
        private readonly DbSet<Appointment> _dbSet;
        public AppointmentRepo(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbSet = dbContext.Set<Appointment>();
        }

    }
}
