namespace Vezeeta_Clone.Service.ExternalServices.Dto
{
    public class MedicalReportPdfDto
    {
        public byte[] LogoBytes { get; set; }
        // Patient Info
        public string PatientName { get; set; }
        public string PatientEmail { get; set; }

        // Doctor Info
        public string DoctorName { get; set; }
        public string Specialization { get; set; }
        public string ClinicName { get; set; }

        // Appointment Info
        public string AppointmentDate { get; set; }
        public string AppointmentTime { get; set; }

        public string VisitNumber { get; set; }
        public string FirstVisitAt { get; set; }
        public string LastVisitAt { get; set; }

        // Diagnosis
        public List<string> Diagnoses { get; set; } = new();

        // Prescription
        public List<PrescriptionItemDto> Medications { get; set; } = new();
        public string? Notes { get; set; }

        // Metadata
        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
    }
}
