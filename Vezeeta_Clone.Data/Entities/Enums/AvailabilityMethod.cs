using System.Runtime.Serialization;

namespace Vezeeta_Clone.Data.Entities.Enums
{
    public enum AvailabilityMethod
    {
        [EnumMember(Value = "Online")]
        Online,
        [EnumMember(Value = "Offline")]
        Offline
    }
}
