using Microsoft.EntityFrameworkCore;
using Vezeeta_Clone.Data.Entities;
using Vezeeta_Clone.Infrastructure.InfrastructureBases;
using Vezeeta_Clone.Service.Abstract;

namespace Vezeeta_Clone.Service.Implementation
{
    public class MedicalRecordService : IMedicalRecordService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MedicalRecordService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<DoctorPatient> GetByDoctorIdAndPatientIdAsync(string doctorId, string patientId)
        {
            var doctorPatient = await _unitOfWork._doctorPatientRepo.GetTableAsTracking()
                            .FirstOrDefaultAsync(dp => dp.DoctorId == doctorId && dp.PatientId == patientId);
            if (doctorPatient == null)
            {
                return null;
            }
            return doctorPatient;
        }


        public async Task<DoctorPatient> VisitDoctor(string doctorId, string patientId)
        {
            var doctorPatient = await GetByDoctorIdAndPatientIdAsync(doctorId, patientId);
            if (doctorPatient != null)
            {
                doctorPatient.TotalVisits++;
                doctorPatient.LastVisitAt = DateTime.UtcNow;
                await _unitOfWork._doctorPatientRepo.UpdateAsync(doctorPatient);
                await _unitOfWork.SaveChangesAsync();
                return doctorPatient;
            }
            doctorPatient = new DoctorPatient
            {
                DoctorId = doctorId,
                PatientId = patientId,
                TotalVisits = 1,
                FirstVisitAt = DateTime.UtcNow,
                LastVisitAt = DateTime.UtcNow
            };
            await _unitOfWork._doctorPatientRepo.AddAsync(doctorPatient);
            await _unitOfWork.SaveChangesAsync();
            return doctorPatient;
        }
        public async Task<MedicalRecord> CreateMedicalRecordAsync(MedicalRecord medicalRecord, string doctorId, string patientId)
        {

            var appointment = await _unitOfWork._appointmentRepo.GetByIntIdAsync(medicalRecord.AppointmentId);
            if (appointment == null || appointment.DoctorId != doctorId || appointment.PatientId != patientId)
            {
                throw new InvalidOperationException("there_is_no_appointment_between");
            }

            var doctorPatient = await VisitDoctor(doctorId, patientId);
            if (doctorPatient == null)
                throw new InvalidOperationException("Failed_to_record_doctor-patient_visit");


            else
            {
                var existingMedicalRecord = await _unitOfWork._medicalRecordRepo.GetTableNoTracking()
                    .Where(mr => mr.DoctorPatientId == doctorPatient.ID && mr.AppointmentId == medicalRecord.AppointmentId)
                    .FirstOrDefaultAsync();

                if (existingMedicalRecord != null)
                    throw new InvalidOperationException("alreadyExists");

                medicalRecord.DoctorPatientId = doctorPatient.ID;

                await _unitOfWork._medicalRecordRepo.AddAsync(medicalRecord);
                await _unitOfWork.SaveChangesAsync();

                return medicalRecord;
            }
        }

        public async Task<Diagnosis> CreateDiagnosisAsync(Diagnosis diagnosis, int medicalRecordId)
        {
            var medicalRecordEntity = await _unitOfWork._medicalRecordRepo.GetByIntIdAsync(medicalRecordId);
            if (medicalRecordEntity != null)
            {
                await _unitOfWork._diagnosisRepo.AddAsync(diagnosis);
                await _unitOfWork.SaveChangesAsync();
                return diagnosis;
            }
            throw new InvalidOperationException("NoMedicalRecordExist");
        }

        public async Task<EPrescription> CreateEPrescriptionAsync(EPrescription ePrescription, List<PrescriptionItem> prescriptionItems, int medicalRecord)
        {
            var medicalRecordEntity = await _unitOfWork._medicalRecordRepo.GetByIntIdAsync(medicalRecord);
            if (medicalRecordEntity != null)
            {
                ePrescription.prescriptions = prescriptionItems;
                await _unitOfWork._prescriptionRepo.AddAsync(ePrescription);
                await _unitOfWork.SaveChangesAsync();
                return ePrescription;
            }
            throw new InvalidOperationException("NoMedicalRecordExist");
        }
        public async Task<MedicalRecord> GetMedicalRecordAsync(int medicalRecordId)
        {
            var medicalRecord = await _unitOfWork._medicalRecordRepo.GetTableNoTracking()
                                    .Include(mr => mr.DoctorPatient)
                                        .ThenInclude(dp => dp.Patient)
                                            .ThenInclude(p => p.ApplicationUser)
                                    .Include(mr => mr.DoctorPatient)
                                        .ThenInclude(dp => dp.Doctor)
                                            .ThenInclude(d => d.ApplicationUser)
                                    .Include(mr => mr.DoctorPatient)
                                        .ThenInclude(dp => dp.Doctor)
                                            .ThenInclude(d => d.Clinic)
                                    .Include(mr => mr.DoctorPatient)
                                        .ThenInclude(dp => dp.Doctor)
                                            .ThenInclude(d => d.Specialization)
                                    .Include(mr => mr.EPrescriptions)
                                        .ThenInclude(ep => ep.prescriptions)
                                    .Include(mr => mr.Diagnoses)
                                    .Include(mr => mr.Appointment)
                                    .ThenInclude(a => a.AvailableSlot)
                                    .FirstOrDefaultAsync(mr => mr.ID == medicalRecordId);

            if (medicalRecord == null)
            {
                return null;
            }
            return medicalRecord;
        }

        public async Task<bool> SaveMedicalRecordReport(string? file_url, int medicalRecordId)
        {
            var medicalRecord = await _unitOfWork._medicalRecordRepo.GetByIntIdAsync(medicalRecordId);
            if (medicalRecord == null || medicalRecord.Diagnoses.Count == 0 || medicalRecord.EPrescriptions.Count == 0)
            {
                return false;
            }
            medicalRecord.FileUrl = file_url;
            await _unitOfWork._medicalRecordRepo.UpdateAsync(medicalRecord);
            await _unitOfWork.SaveChangesAsync();
            return true;

        }

        public async Task<bool> UpdateDiagnosisAsync(int medicalRecordId, string description, string doctorId)
        {
            var medicalRecord = await _unitOfWork._medicalRecordRepo.GetByIntIdAsync(medicalRecordId);
            if (medicalRecord == null || medicalRecord.Diagnoses.Count == 0 || medicalRecord.EPrescriptions.Count == 0)
            {
                return false;
            }

            var diagnosis = _unitOfWork._diagnosisRepo.GetTableAsTracking()
                            .FirstOrDefault(d => d.MedicalRecordId == medicalRecordId);
            if (diagnosis == null)
            {
                return false;
            }
            diagnosis.Description = description;
            diagnosis.UpdatedAt = DateTime.UtcNow;
            await _unitOfWork._diagnosisRepo.UpdateAsync(diagnosis);
            var result = await _unitOfWork.SaveChangesAsync();
            return result > 0;

        }
    }
}
