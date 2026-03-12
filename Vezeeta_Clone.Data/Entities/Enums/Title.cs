using System.Runtime.Serialization;

namespace Vezeeta_Clone.Data.Entities.Enums
{
    public enum Title
    {
        [EnumMember(Value = "Professor")]
        Professor = 1,
        [EnumMember(Value = "Lecturer")]
        Lecturer = 2,
        [EnumMember(Value = "Consultant")]
        Consultant = 3,
        [EnumMember(Value = "Specialist")]
        Specialist = 4

    }
}
