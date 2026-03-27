using Microsoft.EntityFrameworkCore;
using Vezeeta_Clone.Data.Entities;
using Vezeeta_Clone.Infrastructure.InfrastructureBases;
using Vezeeta_Clone.Service.Abstract;

namespace Vezeeta_Clone.Service.Implementation
{
    public class PatientService : IPatientService
    {
        private readonly IUnitOfWork _unitOfWork;
        public PatientService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Patient> GetPatientByAppointmentId(int appointmentId)
        {
            var appointment = await _unitOfWork._appointmentRepo.GetTableNoTracking().Include(a => a.Patient).FirstOrDefaultAsync(a => a.ID == appointmentId);
            if (appointment == null)
                return null;
            var patient = appointment.Patient;
            return patient;
        }
    }
}
