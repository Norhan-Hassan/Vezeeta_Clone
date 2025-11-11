using System.Runtime.Serialization;

namespace Vezeeta_Clone.Data.Entities.Enums
{
    public enum Title
    {
        [EnumMember(Value = "Professor")]
        Professor,
        [EnumMember(Value = "Lecturer")]
        Lecturer,
        [EnumMember(Value = "Consultant")]
        Consultant,
        [EnumMember(Value = "Specialist")]
        Specialist

    }
}
