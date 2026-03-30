using Vezeeta_Clone.Infrastructure.Abstract;

namespace Vezeeta_Clone.Infrastructure.InfrastructureBases
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        public IDoctorRepo _doctorRepo { get; }
        public IAppointmentRepo _appointmentRepo { get; }
        public IDoctorAvailabilityRepo _doctorAvailabilityRepo { get; }
        public IPatientRepo _patientRepo { get; }
        public IClinicRepo _clinicRepo { get; }
        public IDoctorAvailabilitySlotRepo _availabilitySlotRepo { get; }
        public ISpecializationRepo _specializationRepo { get; }
        public ISubSpecializationRepo _subSpecializationRepo { get; }
        public IRefreshTokenRepo _refreshTokenRepo { get; }
        public IReviewRepo _reviewRepo { get; }
        public IDoctorPatientRepo _doctorPatientRepo { get; }
        public IDiagnosisRepo _diagnosisRepo { get; }
        public IPrescriptionRepo _prescriptionRepo { get; }
        public IMedicalRecordRepo _medicalRecordRepo { get; }
        public IPaymentRepo _paymentRepo { get; }
        public IPaymentEventRepo _paymentEventRepo { get; }
        public IClinicImageRepo _clinicImageRepo { get; }
        Task<int> SaveChangesAsync();
    }
}
