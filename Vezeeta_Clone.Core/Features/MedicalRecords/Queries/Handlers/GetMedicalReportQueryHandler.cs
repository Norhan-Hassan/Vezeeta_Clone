using MediatR;
using Microsoft.Extensions.Localization;
using Vezeeta_Clone.Core.Bases;
using Vezeeta_Clone.Core.Features.MedicalRecords.Queries.Models;
using Vezeeta_Clone.Core.Features.MedicalRecords.Queries.Results;
using Vezeeta_Clone.Core.Resources;
using Vezeeta_Clone.Data.Commons;
using Vezeeta_Clone.Data.Helper;
using Vezeeta_Clone.Service.Abstract;
using Vezeeta_Clone.Service.AppUserAuthServices.Abstract;
using Vezeeta_Clone.Service.ExternalServices.Abstract;
using Vezeeta_Clone.Service.ExternalServices.Dto;

namespace Vezeeta_Clone.Core.Features.MedicalRecords.Queries.Handlers
{
    public class GetMedicalReportQueryHandler : ResponseHandler, IRequestHandler<GetMedicalReportQuery, Response<GetMedicalReportQueryResult>>
    {
        #region Fields
        private readonly ICurrentUserService _currentUserService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IMedicalRecordService _medicalRecordService;
        private readonly IPdfGenerationService _pdfService;
        private readonly AzureStorageSettings _azureStorageSettings;
        private readonly IBlobStorageService _blobStorageService;
        #endregion

        #region Constructor
        public GetMedicalReportQueryHandler(IStringLocalizer<SharedResources> localizer,
                                            ICurrentUserService currentUserService,
                                            AzureStorageSettings azureStorageSettings,
                                            IBlobStorageService blobStorageService,
                                            IMedicalRecordService medicalRecordService,
                                            IPdfGenerationService pdfGenerationService) : base(localizer)
        {
            _localizer = localizer;
            _currentUserService = currentUserService;
            _medicalRecordService = medicalRecordService;
            _blobStorageService = blobStorageService;
            _azureStorageSettings = azureStorageSettings;
            _pdfService = pdfGenerationService;
        }
        #endregion

        #region Functions
        public async Task<Response<GetMedicalReportQueryResult>> Handle(GetMedicalReportQuery request, CancellationToken cancellationToken)
        {
            var medicalRecord = await _medicalRecordService.GetMedicalRecordAsync(request.MedicalRecordId);
            if (medicalRecord == null)
            {
                return NotFound<GetMedicalReportQueryResult>(_localizer[SharedResourcesKeys.NotFound]);
            }
            var currentUserRoles = await _currentUserService.GetCurrentUserRolesAsync();
            var currentPatientId = string.Empty;
            if (currentUserRoles.Contains(Roles.Patient))
            {
                currentPatientId = _currentUserService.GetCurrentUserId();
            }
            if (medicalRecord.DoctorPatient.PatientId != currentPatientId)
            {
                return Unauthorized<GetMedicalReportQueryResult>();
            }
            if (medicalRecord.FileUrl != null)
            {
                return Success<GetMedicalReportQueryResult>(new GetMedicalReportQueryResult { DownloadUrl = medicalRecord.FileUrl }, _localizer[SharedResourcesKeys.Success]);
            }
            using var httpClient = new HttpClient();

            var logoBytes = await httpClient.GetByteArrayAsync("https://sehatekstorage.blob.core.windows.net/images/Logo.png");
            var dto = new MedicalReportPdfDto
            {
                LogoBytes = logoBytes,
                PatientName = string.Concat(medicalRecord.DoctorPatient.Patient.ApplicationUser.FirstName, ' ', medicalRecord.DoctorPatient.Patient.ApplicationUser.LastName),
                PatientEmail = medicalRecord.DoctorPatient.Patient.ApplicationUser.Email,
                DoctorName = string.Concat(medicalRecord.DoctorPatient.Doctor.ApplicationUser.FirstName, ' ', medicalRecord.DoctorPatient.Doctor.ApplicationUser.LastName),
                Specialization = medicalRecord.DoctorPatient.Doctor.Specialization.LocalizedName,
                ClinicName = medicalRecord.DoctorPatient.Doctor.Clinic.Name,
                AppointmentDate = medicalRecord.Appointment.AvailableSlot.Date.ToShortDateString(),
                AppointmentTime = medicalRecord.Appointment.AvailableSlot.StartTime.ToString(),
                VisitNumber = medicalRecord.DoctorPatient.TotalVisits.ToString(),
                FirstVisitAt = medicalRecord.DoctorPatient.FirstVisitAt.ToShortDateString(),
                LastVisitAt = medicalRecord.DoctorPatient.LastVisitAt.ToShortDateString(),
                Diagnoses = medicalRecord.Diagnoses.Select(d => d.Description).ToList(),
                Medications = medicalRecord.EPrescriptions
                    .SelectMany(ep => ep.prescriptions)
                    .Select(p => new PrescriptionItemDto { MedicationName = p.Medication, Dosage = p.Dose })
                    .ToList(),
                Notes = medicalRecord.EPrescriptions.FirstOrDefault()?.Notes
            };

            var pdfBytes = _pdfService.GenerateReportPdf(dto);

            var fileName = $"MedicalReport_{medicalRecord.DoctorPatient.Patient.ApplicationUser.FirstName}.pdf";
            var pdfStream = new MemoryStream(pdfBytes);
            var downloadUrl = await _blobStorageService.UploadBlobAsync(_azureStorageSettings.ReportContainer, fileName, pdfStream, "application/pdf");

            await _medicalRecordService.SaveMedicalRecordReport(downloadUrl, request.MedicalRecordId);

            return Success<GetMedicalReportQueryResult>(new GetMedicalReportQueryResult { DownloadUrl = downloadUrl }, _localizer[SharedResourcesKeys.Success]);
        }
        #endregion
    }
}
