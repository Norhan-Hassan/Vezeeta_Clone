using Vezeeta_Clone.Infrastructure.Abstract;
using Vezeeta_Clone.Infrastructure.Context;
using Vezeeta_Clone.Infrastructure.Repos;

namespace Vezeeta_Clone.Infrastructure.InfrastructureBases
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public IDoctorRepo _doctorRepo { get; private set; }
        public IAppointmentRepo _appointmentRepo { get; private set; }
        public IDoctorAvailabilityRepo _doctorAvailabilityRepo { get; private set; }
        public IPatientRepo _patientRepo { get; private set; }
        public IClinicRepo _clinicRepo { get; private set; }
        public IDoctorAvailabilitySlotRepo _availabilitySlotRepo { get; private set; }
        public ISpecializationRepo _specializationRepo { get; private set; }
        public ISubSpecializationRepo _subSpecializationRepo { get; private set; }

        public IRefreshTokenRepo _refreshTokenRepo { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            _doctorRepo = new DoctorRepo(_context);
            _appointmentRepo = new AppointmentRepo(_context);
            _doctorAvailabilityRepo = new DoctorAvailabilityRepo(_context);
            _patientRepo = new PatientRepo(_context);
            _clinicRepo = new ClinicRepo(_context);
            _availabilitySlotRepo = new DoctorAvailabilitySlotRepo(_context);
            _specializationRepo = new SpecializationRepo(_context);
            _subSpecializationRepo = new SubSpecializationRepo(_context);
            _refreshTokenRepo = new RefreshTokenRepo(_context);
        }



        public async ValueTask DisposeAsync()
        {
            await _context.DisposeAsync();
        }

        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}
