using System.Runtime.Serialization;

namespace Vezeeta_Clone.Data.Helper
{
    public enum OrderingCriteria
    {
        [EnumMember(Value = "Top Rated")]
        topRated = 1,
        [EnumMember(Value = "Price Low to High")]
        priceLowToHigh = 2,
        [EnumMember(Value = "Price High to Low")]
        priceHighToLow = 3,
        [EnumMember(Value = "Less Waiting Time")]
        lessWaitingTime = 4,
    }
}
