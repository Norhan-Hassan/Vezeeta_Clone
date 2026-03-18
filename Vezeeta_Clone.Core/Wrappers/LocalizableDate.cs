using Microsoft.Extensions.Localization;
using Vezeeta_Clone.Core.Resources;

namespace Vezeeta_Clone.Core.Wrappers
{
    public static class LocalizableDate
    {
        public static string GetDayName(IStringLocalizer<SharedResources> _localizer, DateOnly date)
        {
            return date.DayOfWeek switch
            {
                DayOfWeek.Saturday => _localizer["Saturday"],
                DayOfWeek.Sunday => _localizer["Sunday"],
                DayOfWeek.Monday => _localizer["Monday"],
                DayOfWeek.Tuesday => _localizer["Tuesday"],
                DayOfWeek.Wednesday => _localizer["Wednesday"],
                DayOfWeek.Thursday => _localizer["Thursday"],
                DayOfWeek.Friday => _localizer["Friday"],
                _ => ""
            };
        }
    }
}
