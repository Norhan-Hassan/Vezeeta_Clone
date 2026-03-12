using Vezeeta_Clone.Data.Entities;
using Vezeeta_Clone.Infrastructure.Context;

namespace Vezeeta_Clone.Infrastructure.Seeder
{
    public static class UniversitySeeder
    {
        public static async Task SeedUniversitiesAsync(ApplicationDbContext context)
        {
            var universityCount = context.Universities.Count();
            if (universityCount == 0)
            {
                var universities = new List<University>
                {
                    new University { NameEn = "Faculty of Medicine - Cairo University - Kasr Al-Ainy", NameAr = "كلية الطب - جامعة القاهرة - القصر العيني" },
                    new University { NameEn = "Faculty of Medicine - Ain Shams University", NameAr = "كلية الطب - جامعة عين شمس" },
                    new University { NameEn = "Faculty of Medicine - Alexandria University", NameAr = "كلية الطب - جامعة الإسكندرية" },
                    new University { NameEn = "Faculty of Medicine - Mansoura University", NameAr = "كلية الطب - جامعة المنصورة" },
                    new University { NameEn = "Faculty of Medicine - Assiut University", NameAr = "كلية الطب - جامعة أسيوط" },
                    new University { NameEn = "Faculty of Medicine - Zagazig University", NameAr = "كلية الطب - جامعة الزقازيق" },
                    new University { NameEn = "Faculty of Medicine - Tanta University", NameAr = "كلية الطب - جامعة طنطا" },
                    new University { NameEn = "Faculty of Medicine - Suez Canal University", NameAr = "كلية الطب - جامعة قناة السويس" },
                    new University { NameEn = "Faculty of Medicine - Benha University", NameAr = "كلية الطب - جامعة بنها" },
                    new University { NameEn = "Faculty of Medicine - Menoufia University", NameAr = "كلية الطب - جامعة المنوفية" },
                    new University { NameEn = "Faculty of Medicine - Fayoum University", NameAr = "كلية الطب - جامعة الفيوم" },
                    new University { NameEn = "Faculty of Medicine - Beni Suef University", NameAr = "كلية الطب - جامعة بني سويف" },
                    new University { NameEn = "Faculty of Medicine - Sohag University", NameAr = "كلية الطب - جامعة سوهاج" },
                    new University { NameEn = "Faculty of Medicine - Minia University", NameAr = "كلية الطب - جامعة المنيا" },
                    new University { NameEn = "Faculty of Medicine - South Valley University", NameAr = "كلية الطب - جامعة جنوب الوادي" },
                    new University { NameEn = "Faculty of Medicine - Aswan University", NameAr = "كلية الطب - جامعة أسوان" },
                    new University { NameEn = "Faculty of Medicine - Helwan University", NameAr = "كلية الطب - جامعة حلوان" },
                    new University { NameEn = "Faculty of Medicine - Kafr El Sheikh University", NameAr = "كلية الطب - جامعة كفر الشيخ" },
                    new University { NameEn = "Faculty of Medicine - Port Said University", NameAr = "كلية الطب - جامعة بورسعيد" },
                    new University { NameEn = "Faculty of Medicine - Suez University", NameAr = "كلية الطب - جامعة السويس" },
                    new University { NameEn = "Faculty of Medicine - Damietta University", NameAr = "كلية الطب - جامعة دمياط" },
                    new University { NameEn = "Faculty of Medicine - New Valley University", NameAr = "كلية الطب - جامعة الوادي الجديد" },
                    new University { NameEn = "Faculty of Medicine - University of Sadat City", NameAr = "كلية الطب - جامعة مدينة السادات" }
                }
                ;
                await context.Universities.AddRangeAsync(universities);
                await context.SaveChangesAsync();
            }
        }
    }
}
