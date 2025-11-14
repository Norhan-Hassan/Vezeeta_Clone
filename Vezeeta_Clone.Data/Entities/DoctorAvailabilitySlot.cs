using System.ComponentModel.DataAnnotations.Schema;

namespace Vezeeta_Clone.Data.Entities
{
    public class DoctorAvailabilitySlot : BaseEntity
    {
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public bool IsBooked { get; set; } = false;
        public string? LockedReason { get; set; }

        [ForeignKey("Availability")]
        public int DoctorAvailabilityId { get; set; }
        public DoctorAvailability? Availability { get; set; }

        //because there would be free solt without apointment
        public Appointment? Appointment { get; set; }


    }
}
