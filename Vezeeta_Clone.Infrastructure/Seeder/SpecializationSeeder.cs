using Microsoft.EntityFrameworkCore;
using Vezeeta_Clone.Data.Entities;
using Vezeeta_Clone.Infrastructure.Context;

namespace Vezeeta_Clone.Infrastructure.Seeder
{
    public static class SpecializationSeeder
    {
        public static async Task SeedSpecializationAsync(ApplicationDbContext context)
        {
            var specCount = await context.Specializations.CountAsync();
            if (specCount == 0)
            {
                var specializations = new List<Specialization>
                {
                    new Specialization { NameEn = "Internal Medicine", NameAr = "الباطنة" },
                    new Specialization { NameEn = "Cardiology", NameAr = "أمراض القلب" },
                    new Specialization { NameEn = "Dermatology", NameAr = "الأمراض الجلدية" },
                    new Specialization { NameEn = "Pediatrics", NameAr = "طب الأطفال" },
                    new Specialization { NameEn = "Neurology", NameAr = "الأمراض العصبية" },
                    new Specialization { NameEn = "Ear, Nose and Throat", NameAr = "الأنف والأذن والحنجرة" },
                    new Specialization { NameEn = "Ophthalmology", NameAr = "طب العيون" },
                    new Specialization { NameEn = "Orthopedics", NameAr = "جراحة العظام" },
                    new Specialization { NameEn = "Gynecology and Obstetrics", NameAr = "النساء والتوليد" },
                    new Specialization { NameEn = "Dentistry", NameAr = "طب الأسنان" },
                    new Specialization { NameEn = "Psychiatry", NameAr = "الطب النفسي" },
                    new Specialization { NameEn = "General Surgery", NameAr = "الجراحة العامة" }
                };

                await context.Specializations.AddRangeAsync(specializations);
                await context.SaveChangesAsync();

                var subSpecializations = new List<SubSpecialization>
                {
                    // Internal Medicine SubSpecialties
                    new SubSpecialization { SpecializationId =1, NameEn = "Cardiovascular Disease", NameAr = "أمراض القلب والأوعية الدموية" },
                    new SubSpecialization { SpecializationId =1, NameEn = "Pulmonology", NameAr = "أمراض الرئة" },
                    new SubSpecialization { SpecializationId = 1, NameEn = "Nephrology", NameAr = "أمراض الكلى" },
                    new SubSpecialization { SpecializationId =1, NameEn = "Rheumatology", NameAr = "الروماتيزم وأمراض المفاصل" },
                    new SubSpecialization { SpecializationId =1, NameEn = "Hematology", NameAr = "أمراض الدم" },

                    // Cardiology SubSpecialties
                    new SubSpecialization { SpecializationId = 2, NameEn = "Interventional Cardiology", NameAr = "أمراض القلب التداخلية" },
                    new SubSpecialization { SpecializationId = 2, NameEn = "Electrophysiology", NameAr = "تنظيم كهرباء القلب" },
                    new SubSpecialization { SpecializationId = 2, NameEn = "Heart Failure & Transplant", NameAr = "فشل القلب وزراعة القلب" },

                    // Dermatology SubSpecialties
                    new SubSpecialization { SpecializationId = 3, NameEn = "Cosmetic Dermatology", NameAr = "الأمراض الجلدية التجميلية" },
                    new SubSpecialization { SpecializationId = 3, NameEn = "Pediatric Dermatology", NameAr = "جلدية الأطفال" },
                    new SubSpecialization { SpecializationId = 3, NameEn = "Dermatopathology", NameAr = "جلدية مختبرية" },

                    // Pediatrics SubSpecialties
                    new SubSpecialization { SpecializationId = 4, NameEn = "Neonatal Medicine", NameAr = "طب حديثي الولادة" },
                    new SubSpecialization { SpecializationId = 4, NameEn = "Pediatric Cardiology", NameAr = "أمراض قلب الأطفال" },
                    new SubSpecialization { SpecializationId =4, NameEn = "Pediatric Endocrinology", NameAr = "غدد صماء الأطفال" },

                    // Neurology SubSpecialties
                    new SubSpecialization { SpecializationId = 5, NameEn = "Clinical Neurophysiology", NameAr = "فيزيولوجيا الأعصاب" },
                    new SubSpecialization { SpecializationId = 5, NameEn = "Neurocritical Care", NameAr = "رعاية حرجة عصبية" },

                    // ENT (Ear, Nose and Throat) SubSpecialties
                    new SubSpecialization { SpecializationId = 6, NameEn = "Pediatric ENT", NameAr = "أنف وأذن وحنجرة للأطفال" },
                    new SubSpecialization { SpecializationId = 6, NameEn = "Otologic Surgery", NameAr = "جراحة الأذن" },

                    // Ophthalmology SubSpecialties
                    new SubSpecialization { SpecializationId = 7, NameEn = "Retina Specialist", NameAr = "أمراض الشبكية" },
                    new SubSpecialization { SpecializationId = 7, NameEn = "Glaucoma Specialist", NameAr = "أمراض الزرق" },

                     // Orthopedics SubSpecialties
                    new SubSpecialization { SpecializationId = 8, NameEn = "Spine Surgery", NameAr = "جراحة العمود الفقري" },
                    new SubSpecialization { SpecializationId = 8, NameEn = "Sports Medicine", NameAr = "طب إصابات الملاعب" },

                    // Gynecology & Obstetrics SubSpecialties
                    new SubSpecialization { SpecializationId =9, NameEn = "High-Risk Pregnancy", NameAr = "الحمل عالي الخطورة" },
                    new SubSpecialization { SpecializationId = 9, NameEn = "Reproductive Endocrinology", NameAr = "غدد التناسلية والتوليد" },

                   
                    // Dentistry SubSpecialties

                    new SubSpecialization { SpecializationId = 10, NameEn = "Orthodontics", NameAr = "تقويم الأسنان" },
                    new SubSpecialization { SpecializationId = 10, NameEn = "Oral Surgery", NameAr = "جراحة الفم" },
                    new SubSpecialization { SpecializationId = 10, NameEn = "Pediatric Dentistry", NameAr = "طب أسنان الأطفال" },
                    new SubSpecialization { SpecializationId = 10, NameEn = "Prosthodontics", NameAr = "تركيب الأسنان التعويضي" },
                    new SubSpecialization { SpecializationId =10, NameEn = "Endodontics", NameAr = "عصب الأسنان" },

                    // Psychiatry SubSpecialties
                    new SubSpecialization { SpecializationId = 11, NameEn = "Child and Adolescent Psychiatry", NameAr = "طب نفسي للأطفال والمراهقين" },
                    new SubSpecialization { SpecializationId = 11, NameEn = "Forensic Psychiatry", NameAr = "الطب النفسي الجنائي" },
                    new SubSpecialization { SpecializationId = 11, NameEn = "Addiction Psychiatry", NameAr = "طب نفسي لعلاج الإدمان" },
                    new SubSpecialization { SpecializationId = 11, NameEn = "Geriatric Psychiatry", NameAr = "طب نفسي لكبار السن" },
                    new SubSpecialization { SpecializationId = 11, NameEn = "Consultation-Liaison Psychiatry", NameAr = "الاستشارة النفسية في المستشفيات" },

                    new SubSpecialization { SpecializationId = 12, NameEn = "Trauma Surgery", NameAr = "جراحة الإصابات" },
                    new SubSpecialization { SpecializationId = 12, NameEn = "Colorectal Surgery", NameAr = "جراحة القولون والمستقيم" },
                    new SubSpecialization { SpecializationId = 12, NameEn = "Hepatobiliary Surgery", NameAr = "جراحة الكبد والمرارة" },
                    new SubSpecialization { SpecializationId = 12, NameEn = "Endocrine Surgery", NameAr = "جراحة الغدد الصماء" },
                    new SubSpecialization { SpecializationId = 12, NameEn = "Minimally Invasive Surgery", NameAr = "الجراحة بالمنظار" },
                };

                await context.SubSpecializations.AddRangeAsync(subSpecializations);
                await context.SaveChangesAsync();
            }
        }
    }
}
