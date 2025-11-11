using System.ComponentModel.DataAnnotations.Schema;
using Vezeeta_Clone.Data.Entities.Enums;

namespace Vezeeta_Clone.Data.Entities
{
    public class Appointment : BaseEntity
    {
        public Status Status { get; set; } = Status.Upcoming;

        [ForeignKey("Patient")]
        public string PatientId { get; set; }
        public Patient? Patient { get; set; }

        [ForeignKey("AvailableSlot")]
        public int SlotId { get; set; }
        public DoctorAvailabilitySlot? AvailableSlot { get; set; }


    }
}
