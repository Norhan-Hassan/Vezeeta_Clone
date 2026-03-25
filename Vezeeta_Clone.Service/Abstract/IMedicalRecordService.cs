using Vezeeta_Clone.Data.Entities;

namespace Vezeeta_Clone.Service.Abstract
{
    public interface IMedicalRecordService
    {
        Task<DoctorPatient> GetByDoctorIdAndPatientIdAsync(string doctorId, string patientId);
        Task<DoctorPatient> VisitDoctor(string doctorId, string patientId);
        Task<MedicalRecord> CreateMedicalRecordAsync(MedicalRecord medicalRecord, string doctorId, string patientId);
        Task<Diagnosis> CreateDiagnosisAsync(Diagnosis diagnosis, int medicalRecord);
        Task<EPrescription> CreateEPrescriptionAsync(EPrescription ePrescription, List<PrescriptionItem> prescriptionItems, int medicalRecord);
    }
}
