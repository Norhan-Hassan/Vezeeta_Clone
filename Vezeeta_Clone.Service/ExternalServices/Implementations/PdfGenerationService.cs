using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Vezeeta_Clone.Service.ExternalServices.Abstract;
using Vezeeta_Clone.Service.ExternalServices.Dto;

namespace Vezeeta_Clone.Service.ExternalServices.Implementations
{
    public class PdfGenerationService : IPdfGenerationService
    {
        private const string PrimaryDark = "#22577A";
        private const string PrimaryMedium = "#38A3A5";
        private const string PrimaryLight = "#57CC99";
        private const string SecondaryLight = "#80ED99";
        private const string BackgroundLight = "#cae3cc";

        public PdfGenerationService()
        {
            QuestPDF.Settings.License = LicenseType.Community;
            QuestPDF.Settings.EnableDebugging = true;
        }

        public byte[] GenerateReportPdf(MedicalReportPdfDto data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(0);

                    page.DefaultTextStyle(x =>
                        x.FontSize(11)
                         .FontFamily("Segoe UI", "Arial"));

                    page.Header().Background(PrimaryDark)
                        .PaddingLeft(25).PaddingTop(20).PaddingRight(25).PaddingBottom(20)
                        .Element(c => ComposeHeader(c, data));

                    page.Content().Padding(25).Element(c => ComposeContent(c, data));


                    page.Footer().PaddingLeft(25).PaddingTop(20).PaddingRight(25).PaddingBottom(20).Element(ComposeFooter);
                });
            });

            return document.GeneratePdf();
        }

        private void ComposeHeader(IContainer container, MedicalReportPdfDto data)
        {
            container.Row(row =>
            {
                if (data.LogoBytes != null)
                {
                    row.ConstantItem(40)
                       .Height(40)
                       .Image(data.LogoBytes).FitArea();
                }

                row.RelativeItem().PaddingHorizontal(15).Column(col =>
                {
                    col.Item().Text("Sehatek.com")
                        .FontSize(14)
                        .Bold()
                        .FontColor("#FFFFFF");

                    col.Item().Text("Medical Excellence")
                        .FontSize(8)
                        .FontColor(SecondaryLight);
                });

                row.ConstantItem(90).AlignRight().Column(col =>
                {
                    col.Item().Text("MEDICAL REPORT")
                        .FontSize(12)
                        .Bold()
                        .FontColor("#FFFFFF");
                });
            });
        }

        private void ComposeContent(IContainer container, MedicalReportPdfDto data)
        {
            container.Column(column =>
            {
                column.Spacing(12);

                // ===== PATIENT INFORMATION SECTION =====
                column.Item().Element(c => ComposePatientInfo(c, data));

                // ===== APPOINTMENT SECTION =====
                column.Item().Element(c => ComposeAppointmentDetails(c, data));

                // ===== DIAGNOSIS SECTION =====
                if (data.Diagnoses?.Any() == true)
                {
                    column.Item().Element(c => ComposeDiagnosis(c, data));
                }

                // ===== MEDICATIONS SECTION =====
                if (data.Medications?.Any() == true)
                {
                    column.Item().Element(c => ComposeMedications(c, data));
                }
            });
        }

        private void ComposePatientInfo(IContainer container, MedicalReportPdfDto data)
        {
            container.Column(column =>
            {
                // Section Header
                column.Item().Row(row =>
                {
                    row.RelativeItem().Column(col =>
                    {
                        col.Item().Text("Patient Information")
                            .FontSize(11)
                            .SemiBold()
                            .FontColor(PrimaryDark);
                    });

                    row.ConstantItem(3).Background(PrimaryLight);
                });

                column.Item().Height(8); // Spacing

                // Patient & Doctor Info Grid
                column.Item().Row(row =>
                {
                    // Patient Column
                    row.RelativeItem().PaddingRight(15).Column(patientCol =>
                    {
                        patientCol.Item().Text("Patient")
                            .FontSize(8)
                            .Bold()
                            .FontColor(PrimaryMedium);

                        patientCol.Item().Text(data.PatientName)
                            .FontSize(11)
                            .SemiBold()
                            .FontColor(PrimaryDark);

                        patientCol.Item().Height(6);

                        patientCol.Item().Text("Email")
                            .FontSize(7)
                            .Bold()
                            .FontColor(PrimaryMedium);

                        patientCol.Item().Text(data.PatientEmail)
                            .FontSize(9)
                            .FontColor("#666");
                    });

                    // Doctor Column
                    row.RelativeItem().Column(doctorCol =>
                    {
                        doctorCol.Item().Text("Practitioner")
                            .FontSize(8)
                            .Bold()
                            .FontColor(PrimaryMedium);

                        doctorCol.Item().Text($"Dr. {data.DoctorName}")
                            .FontSize(11)
                            .SemiBold()
                            .FontColor(PrimaryDark);

                        doctorCol.Item().Height(6);

                        doctorCol.Item().Row(specRow =>
                        {
                            specRow.RelativeItem().Column(specCol =>
                            {
                                specCol.Item().Text("Specialization")
                                    .FontSize(7)
                                    .Bold()
                                    .FontColor(PrimaryMedium);

                                specCol.Item().Text(data.Specialization)
                                    .FontSize(9)
                                    .FontColor("#666");
                            });

                            specRow.RelativeItem().Column(clinicCol =>
                            {
                                clinicCol.Item().Text("Clinic")
                                    .FontSize(7)
                                    .Bold()
                                    .FontColor(PrimaryMedium);

                                clinicCol.Item().Text(data.ClinicName)
                                    .FontSize(9)
                                    .FontColor("#666");
                            });
                        });
                    });
                });

                // Decorative bottom line
                column.Item().Height(1).Background(SecondaryLight);
            });
        }

        private void ComposeAppointmentDetails(IContainer container, MedicalReportPdfDto data)
        {
            container.Column(column =>
            {
                // Section Header
                column.Item().Row(row =>
                {
                    row.RelativeItem().Column(col =>
                    {
                        col.Item().Text("Appointment")
                            .FontSize(11)
                            .SemiBold()
                            .FontColor(PrimaryDark);
                    });

                    row.ConstantItem(3).Background(PrimaryMedium);
                });

                column.Item().Height(8); // Spacing

                // Appointment Details
                column.Item().Row(row =>
                {
                    row.RelativeItem().Column(dateCol =>
                    {
                        dateCol.Item().Text("Date")
                            .FontSize(8)
                            .Bold()
                            .FontColor(PrimaryMedium);

                        dateCol.Item().Text(data.AppointmentDate)
                            .FontSize(10)
                            .FontColor(PrimaryDark);
                    });

                    row.RelativeItem().Column(timeCol =>
                    {
                        timeCol.Item().Text("Time")
                            .FontSize(8)
                            .Bold()
                            .FontColor(PrimaryMedium);

                        timeCol.Item().Text(data.AppointmentTime)
                            .FontSize(10)
                            .FontColor(PrimaryDark);
                    });
                });

                column.Item().Height(8); // Spacing

                // VisitDetails Details
                column.Item().Row(row =>
                {
                    row.RelativeItem().Column(dateCol =>
                    {
                        dateCol.Item().Text("Visit Number")
                            .FontSize(8)
                            .Bold()
                            .FontColor(PrimaryMedium);

                        dateCol.Item().Text(data.VisitNumber)
                            .FontSize(10)
                            .FontColor(PrimaryDark);
                    });

                    row.RelativeItem().Column(timeCol =>
                    {
                        timeCol.Item().Text("First Visit At")
                            .FontSize(8)
                            .Bold()
                            .FontColor(PrimaryMedium);

                        timeCol.Item().Text(data.FirstVisitAt)
                            .FontSize(10)
                            .FontColor(PrimaryDark);
                    });
                    row.RelativeItem().Column(timeCol =>
                    {
                        timeCol.Item().Text("Last Visit At")
                            .FontSize(8)
                            .Bold()
                            .FontColor(PrimaryMedium);

                        timeCol.Item().Text(data.LastVisitAt)
                            .FontSize(10)
                            .FontColor(PrimaryDark);
                    });
                });

                // Decorative bottom line
                column.Item().Height(1).Background(SecondaryLight);
            });
        }

        private void ComposeDiagnosis(IContainer container, MedicalReportPdfDto data)
        {
            container.Column(column =>
            {
                // Section Header with accent
                column.Item().Row(row =>
                {
                    row.RelativeItem().Column(col =>
                    {
                        col.Item().Text("Diagnosis")
                            .FontSize(11)
                            .SemiBold()
                            .FontColor(PrimaryDark);
                    });

                    row.ConstantItem(3).Background(PrimaryLight);
                });

                column.Item().Height(8);

                // Diagnoses List
                column.Item().Column(diagCol =>
                {
                    diagCol.Spacing(4);

                    foreach (var diagnosis in data.Diagnoses)
                    {
                        diagCol.Item().Row(diagRow =>
                        {
                            diagRow.ConstantItem(14).AlignCenter().AlignMiddle()
                                .Text("•")
                                .FontSize(12)
                                .FontColor(PrimaryMedium);

                            diagRow.RelativeItem().PaddingLeft(6)
                                .Text(diagnosis)
                                .FontSize(9)
                                .FontColor("#333");
                        });
                    }
                });

                // Decorative bottom line
                column.Item().Height(1).Background(SecondaryLight);
            });
        }

        private void ComposeMedications(IContainer container, MedicalReportPdfDto data)
        {
            container.Column(column =>
            {
                // Section Header
                column.Item().Row(row =>
                {
                    row.RelativeItem().Column(col =>
                    {
                        col.Item().Text("Medcations")
                            .FontSize(11)
                            .SemiBold()
                            .FontColor(PrimaryDark);
                    });

                    row.ConstantItem(3).Background(PrimaryLight);
                });

                column.Item().Height(8);

                // Medications Table Style
                column.Item().Column(medCol =>
                {
                    medCol.Spacing(6);

                    int index = 0;
                    foreach (var medication in data.Medications)
                    {
                        if (index > 0)
                        {
                            medCol.Item().Height(1).Background(BackgroundLight);
                        }

                        medCol.Item().Row(medRow =>
                        {
                            // Medication name
                            medRow.RelativeItem(2).Column(nameCol =>
                            {
                                nameCol.Item().Text("Medication")
                                    .FontSize(7)
                                    .Bold()
                                    .FontColor(PrimaryMedium);

                                nameCol.Item().Text(medication.MedicationName)
                                    .FontSize(10)
                                    .SemiBold()
                                    .FontColor(PrimaryDark);
                            });

                            // Dosage
                            medRow.RelativeItem(1).Column(dosageCol =>
                            {
                                dosageCol.Item().Text("Dosage")
                                    .FontSize(7)
                                    .Bold()
                                    .FontColor(PrimaryMedium);

                                dosageCol.Item().Text(medication.Dosage)
                                    .FontSize(9)
                                    .FontColor("#333");
                            });
                        });

                        index++;
                    }
                });

                // Notes Section
                if (!string.IsNullOrWhiteSpace(data.Notes))
                {
                    column.Item().Height(8);

                    column.Item().Background(BackgroundLight)
                        .Padding(10)
                        .Column(notesCol =>
                        {
                            notesCol.Item().Text("Additional Notes")
                                .FontSize(8)
                                .Bold()
                                .FontColor(PrimaryMedium);

                            notesCol.Item().Height(4);

                            notesCol.Item().Text(data.Notes)
                                .FontSize(9)
                                .FontColor("#333")
                                .LineHeight(1.3f);
                        });
                }

                // Final decorative line
                column.Item().Height(1).Background(SecondaryLight);
            });
        }

        private void ComposeFooter(IContainer container)
        {
            container.Column(column =>
            {
                column.Item().Height(1).Background(PrimaryLight);

                column.Item().Height(8);

                column.Item().Row(row =>
                {
                    row.RelativeItem().Column(col =>
                    {
                        col.Item().Text("Sehatek.com")
                            .FontSize(8)
                            .Bold()
                            .FontColor(PrimaryDark);

                        col.Item().Text($"Generated: {DateTime.UtcNow:dd MMM yyyy, HH:mm:ss}")
                            .FontSize(7)
                            .FontColor(PrimaryMedium);
                    });

                    row.ConstantItem(70).AlignRight().Column(col =>
                    {
                        col.Item().Text("Confidential")
                            .FontSize(7)
                            .Bold()
                            .FontColor(PrimaryMedium);

                        col.Item().Text("Medical Document")
                            .FontSize(7)
                            .FontColor("#999");
                    });
                });
            });
        }
    }
}