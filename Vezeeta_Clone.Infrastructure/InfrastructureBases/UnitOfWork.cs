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
        public IReviewRepo _reviewRepo { get; private set; }
        public IRefreshTokenRepo _refreshTokenRepo { get; private set; }
        public IDoctorPatientRepo _doctorPatientRepo { get; private set; }
        public IDiagnosisRepo _diagnosisRepo { get; private set; }
        public IPrescriptionRepo _prescriptionRepo { get; private set; }
        public IMedicalRecordRepo _medicalRecordRepo { get; private set; }
        public IPaymentRepo _paymentRepo { get; private set; }
        public IPaymentEventRepo _paymentEventRepo { get; private set; }
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
            _reviewRepo = new ReviewRepo(_context);
            _doctorPatientRepo = new DoctorPatientRepo(_context);
            _diagnosisRepo = new DiagnosisRepo(_context);
            _prescriptionRepo = new PrescriptionRepo(_context);
            _medicalRecordRepo = new MedicalRecordRepo(_context);
            _paymentRepo = new PaymentRepo(_context);
            _paymentEventRepo = new PaymentEventRepo(_context);
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
