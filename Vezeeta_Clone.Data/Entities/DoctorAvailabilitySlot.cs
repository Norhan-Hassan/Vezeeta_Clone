using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Vezeeta_Clone.Data.Entities.Enums;

namespace Vezeeta_Clone.Data.Entities
{
    public class DoctorAvailabilitySlot : BaseEntity
    {
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public DateOnly Date { get; set; }
        public SlotStatus Status { get; set; } = SlotStatus.Available;
        public bool IsBooked { get; set; } = false;
        public string? LockedReason { get; set; }

        [ForeignKey("Availability")]
        public int DoctorAvailabilityId { get; set; }
        public DoctorAvailability? Availability { get; set; }

        //because there would be free solt without apointment(may =be)
        //public Appointment? Appointment { get; set; }
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        [Timestamp]
        public byte[] RowVersion { get; set; }  // Concurrency token
    }
}
