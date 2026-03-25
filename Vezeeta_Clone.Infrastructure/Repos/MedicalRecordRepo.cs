using Microsoft.EntityFrameworkCore;
using Vezeeta_Clone.Data.Entities;
using Vezeeta_Clone.Infrastructure.Abstract;
using Vezeeta_Clone.Infrastructure.Context;
using Vezeeta_Clone.Infrastructure.InfrastructureBases;

namespace Vezeeta_Clone.Infrastructure.Repos
{
    public class MedicalRecordRepo : GenericRepositoryAsync<MedicalRecord>, IMedicalRecordRepo
    {
        private readonly DbSet<MedicalRecord> _medicalRecord;
        public MedicalRecordRepo(ApplicationDbContext dbContext) : base(dbContext)
        {
            _medicalRecord = _dbContext.Set<MedicalRecord>();
        }
    }
}
