using Microsoft.EntityFrameworkCore;
using Vezeeta_Clone.Data.Entities;
using Vezeeta_Clone.Infrastructure.Context;

namespace Vezeeta_Clone.Infrastructure.Seeder
{
    public static class CityRegionSeeder
    {
        public async static Task SeedCityRegionsAsync(ApplicationDbContext context)
        {
            var cityCount = await context.City.CountAsync();
            if (cityCount == 0)
            {
                var cities = new List<City>
                {
                    new City { NameEn = "Cairo", NameAr = "القاهرة" },
                    new City { NameEn = "Giza", NameAr = "الجيزة" },
                    new City { NameEn = "Alexandria", NameAr = "الإسكندرية" },
                    new City { NameEn = "Qalyubia", NameAr = "القليوبية" },
                    new City { NameEn = "Sharqia", NameAr = "الشرقية" },
                    new City { NameEn = "Dakahlia", NameAr = "الدقهلية" },
                    new City { NameEn = "Gharbia", NameAr = "الغربية" },
                    new City { NameEn = "Monufia", NameAr = "المنوفية" },
                    new City { NameEn = "Fayoum", NameAr = "الفيوم" },
                    new City { NameEn = "Luxor", NameAr = "الأقصر" },
                    new City { NameEn = "Aswan", NameAr = "أسوان" },
                    new City { NameEn = "Red Sea", NameAr = "البحر الأحمر" }
                };
                await context.City.AddRangeAsync(cities);
                await context.SaveChangesAsync();

            }
            var regionCount = await context.Regions.CountAsync();
            if (regionCount == 0)
            {
                var regions = new List<Region>
                {
                    //cairo
                    new Region { NameEn = "Nasr City", NameAr = "مدينة نصر", CityId = 1 },
                    new Region { NameEn = "Heliopolis", NameAr = "مصر الجديدة", CityId = 1 },
                    new Region { NameEn = "New Cairo", NameAr = "القاهرة الجديدة", CityId = 1 },
                    new Region { NameEn = "Maadi", NameAr = "المعادي", CityId = 1 },
                    new Region { NameEn = "Zamalek", NameAr = "الزمالك", CityId = 1 },
                    new Region { NameEn = "Garden City", NameAr = "جاردن سيتي", CityId = 1 },
                    new Region { NameEn = "El-Obour City", NameAr = "العبور", CityId = 1 },
                    new Region { NameEn = "El-Manayal", NameAr = "المنيل", CityId = 1 },
                    new Region { NameEn = "Dar El-Salam", NameAr = "دار السلام", CityId = 1 },

                    new Region { NameEn = "Downtown", NameAr = "وسط البلد", CityId = 1 },
                    new Region { NameEn = "Shubra", NameAr = "شبرا", CityId = 1 },
                    new Region { NameEn = "Helwan", NameAr = "حلوان", CityId = 1 },
                    new Region { NameEn = "El Marg", NameAr = "المرج", CityId = 1 },
                    new Region { NameEn = "Ain Shams", NameAr = "عين شمس", CityId = 1 },
                    new Region { NameEn = "Mataria", NameAr = "المطرية", CityId = 1 },
                    new Region { NameEn = "El Rehab", NameAr = "الرحاب", CityId = 1 },
                    new Region { NameEn = "Madinaty", NameAr = "مدينتي", CityId = 1 },
                    new Region { NameEn = "Badr City", NameAr = "مدينة بدر", CityId = 1 },
                    //giza
                    new Region { NameEn = "Dokki", NameAr = "الدقي", CityId = 2 },
                    new Region { NameEn = "Mohandessin", NameAr = "المهندسين", CityId = 2 },
                    new Region { NameEn = "Agouza", NameAr = "العجوزة", CityId = 2 },
                    new Region { NameEn = "Haram", NameAr = "الهرم", CityId = 2 },
                    new Region { NameEn = "Faisal", NameAr = "فيصل", CityId = 2 },
                    new Region { NameEn = "El-Sheikh Zayed", NameAr = "الشيخ زايد", CityId = 2 },
                    new Region { NameEn = "6th of October", NameAr = "السادس من أكتوبر", CityId = 2 },
                    new Region { NameEn = "Badrashein", NameAr = "البدرشين", CityId = 2 },
                    new Region { NameEn = "Hawamdya", NameAr = "الحوامديه", CityId = 2 },
                    new Region { NameEn = "Giza Square", NameAr = "ميدان الجيزه", CityId = 2 },
                    new Region { NameEn = "New Giza", NameAr = "الجيزه الجديده", CityId = 2 },
                    new Region { NameEn = "Hadayek Al Ahram", NameAr = "حدائق الاهرام", CityId = 2 },
                    new Region { NameEn = "Bulaq Dakrour", NameAr = "بولاق الدكرور", CityId = 2 },
                    new Region { NameEn = "Imbaba", NameAr = "امبابة", CityId = 2 },
                    new Region { NameEn = "Warraq", NameAr = "الوراق", CityId = 2 },
                    new Region { NameEn = "Kerdasa", NameAr = "كرداسة", CityId = 2 },
                    //Alexandria
                    new Region { NameEn = "Smouha", NameAr = "سموحة", CityId = 3 },
                    new Region { NameEn = "Stanley", NameAr = "ستانلي", CityId = 3 },
                    new Region { NameEn = "Miami", NameAr = "ميامي", CityId = 3 },
                    new Region { NameEn = "Montaza", NameAr = "المنتزه", CityId = 3 },
                    new Region { NameEn = "Gleem", NameAr = "جليم", CityId = 3 },
                    //
                    new Region { NameEn = "Zagazig", NameAr = "الزقازيق", CityId = 5 },
                    new Region { NameEn = "10th of Ramadan", NameAr = "العاشر من رمضان", CityId = 5 },
                    new Region { NameEn = "Belbeis", NameAr = "بلبيس", CityId = 5 },
                    new Region { NameEn = "Mansoura", NameAr = "المنصورة", CityId = 6 },
                    new Region { NameEn = "Talkha", NameAr = "طلخا", CityId = 6 },
                    new Region { NameEn = "Tanta", NameAr = "طنطا", CityId = 7 },
                    new Region { NameEn = "Mahalla", NameAr = "المحلة الكبرى", CityId = 7 },
                    new Region { NameEn = "Shebin El Kom", NameAr = "شبين الكوم", CityId = 8 },
                    new Region { NameEn = "Menouf", NameAr = "منوف", CityId = 8 },
                    new Region { NameEn = "Fayoum City", NameAr = "مدينة الفيوم", CityId = 9 },
                    new Region { NameEn = "Ibshaway", NameAr = "ابشواي", CityId = 9 },
                    new Region { NameEn = "Luxor City", NameAr = "مدينة الأقصر", CityId = 10 },
                    new Region { NameEn = "Aswan City", NameAr = "مدينة أسوان", CityId = 11 },
                    new Region { NameEn = "KumUmbo", NameAr = "كوم اومبو", CityId = 11 },
                    new Region { NameEn = "Edfo", NameAr = "ادفو", CityId = 11 },
                    new Region { NameEn = "Hurghada", NameAr = "الغردقة", CityId = 12 },
                    new Region { NameEn = "El Gouna", NameAr = "الجونة", CityId = 12 }
                };
                await context.Regions.AddRangeAsync(regions);
                await context.SaveChangesAsync();
            }
        }
    }
}
