using System.Runtime.Serialization;

namespace Vezeeta_Clone.Data.Entities.Enums
{
    public enum BloodType
    {
        [EnumMember(Value = "A+")]
        Apositive,
        [EnumMember(Value = "A-")]
        Anegative,
        [EnumMember(Value = "B+")]
        Bpositive,
        [EnumMember(Value = "B-")]
        Bnegative,
        [EnumMember(Value = "AB+")]
        ABpositive,
        [EnumMember(Value = "AB-")]
        ABnegative,
        [EnumMember(Value = "O+")]
        Opositive,
        [EnumMember(Value = "O-")]
        Onegative
    }
}
