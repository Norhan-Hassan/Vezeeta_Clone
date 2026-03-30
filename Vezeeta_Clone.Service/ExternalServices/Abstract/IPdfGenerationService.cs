using Vezeeta_Clone.Service.ExternalServices.Dto;

namespace Vezeeta_Clone.Service.ExternalServices.Abstract
{
    public interface IPdfGenerationService
    {
        byte[] GenerateReportPdf(MedicalReportPdfDto medicalReportPdfDto);
    }
}
//var record = await _context.MedicalRecords
//    .Include(m => m.DoctorPatient)
//        .ThenInclude(dp => dp.Doctor)
//    .Include(m => m.DoctorPatient)
//        .ThenInclude(dp => dp.Patient)
//    .Include(m => m.Diagnoses)
//    .Include(m => m.EPrescriptions)
//        .ThenInclude(p => p.prescriptions)
//    .FirstOrDefaultAsync(m => m.Id == id);