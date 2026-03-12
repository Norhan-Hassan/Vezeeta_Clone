using System.Runtime.Serialization;

namespace Vezeeta_Clone.Data.Entities.Enums
{
    public enum Gender
    {
        [EnumMember(Value = "Male")]
        Male = 1,
        [EnumMember(Value = "Female")]
        Female = 2
    }
}
