using System.Runtime.Serialization;

namespace Vezeeta_Clone.Data.Entities.Enums
{
    public enum AvailabilityMethod
    {


        [EnumMember(Value = "Offline")]
        Offline = 1,
        [EnumMember(Value = "Online")]
        Online = 2,
    }
}
