using System.Globalization;

namespace Vezeeta_Clone.Data.Commons
{
    public class LocalizableEntity
    {
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
