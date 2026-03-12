using System.Runtime.Serialization;

namespace Vezeeta_Clone.Data.Entities.Enums
{
    public enum BloodType
    {
        [EnumMember(Value = "A+")]
        Apositive = 1,
        [EnumMember(Value = "A-")]
        Anegative = 2,
        [EnumMember(Value = "B+")]
        Bpositive = 3,
        [EnumMember(Value = "B-")]
        Bnegative = 4,
        [EnumMember(Value = "AB+")]
        ABpositive = 5,
        [EnumMember(Value = "AB-")]
        ABnegative = 6,
        [EnumMember(Value = "O+")]
        Opositive = 7,
        [EnumMember(Value = "O-")]
        Onegative = 8
    }
}
