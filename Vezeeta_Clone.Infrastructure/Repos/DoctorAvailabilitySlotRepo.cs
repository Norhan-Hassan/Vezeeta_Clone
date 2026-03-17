using Microsoft.EntityFrameworkCore;
using Vezeeta_Clone.Data.Entities;
using Vezeeta_Clone.Infrastructure.Abstract;
using Vezeeta_Clone.Infrastructure.Context;
using Vezeeta_Clone.Infrastructure.InfrastructureBases;

namespace Vezeeta_Clone.Infrastructure.Repos
{
    public class DoctorAvailabilitySlotRepo : GenericRepositoryAsync<DoctorAvailabilitySlot>, IDoctorAvailabilitySlotRepo
    {
        private readonly DbSet<DoctorAvailabilitySlot> _doctorAvailabilitySlotRepo;
        public DoctorAvailabilitySlotRepo(ApplicationDbContext dbContext) : base(dbContext)
        {
            _doctorAvailabilitySlotRepo = dbContext.Set<DoctorAvailabilitySlot>();
        }
    }
}
