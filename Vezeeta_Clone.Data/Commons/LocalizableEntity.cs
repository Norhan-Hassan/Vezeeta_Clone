using System.Globalization;

namespace Vezeeta_Clone.Data.Commons
{
    public class LocalizableEntity
    {
        // will be added in each entity that needs localization
        //public string NameAr { get; set; }
        //public string NameEn { get; set; }

        public string GetLocalizedName(string textEn, string textAr)
        {
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            return currentCulture.TwoLetterISOLanguageName.ToLower() switch
            {
                "ar" => textAr,
                _ => textEn,
            };
        }
    }
}
