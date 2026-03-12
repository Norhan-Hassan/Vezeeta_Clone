using System.Runtime.Serialization;

namespace Vezeeta_Clone.Data.Entities.Enums
{
    public enum AvailabilityMethod
    {
        [EnumMember(Value = "Online")]
        Online = 1,
        [EnumMember(Value = "Offline")]
        Offline = 2
    }
}
