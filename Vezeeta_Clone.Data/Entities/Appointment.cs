using System.ComponentModel.DataAnnotations.Schema;
using Vezeeta_Clone.Data.Entities.Enums;

namespace Vezeeta_Clone.Data.Entities
{
    public class Appointment : BaseEntity
    {
        public string? ActualPatientName { get; set; }
        public string? ActualPatientPhone { get; set; }
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Upcoming;
        public DateTime BookedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ConfirmedAt { get; set; }
        public DateTime? CancelledAt { get; set; }
        public DateTime? CompletedAt { get; set; }

        public string? Notes { get; set; }
        public string? CancellationReason { get; set; }

        [ForeignKey("Patient")]
        public string PatientId { get; set; }
        public Patient? Patient { get; set; }

        [ForeignKey("Doctor")]
        public string DoctorId { get; set; }
        public Doctor? Doctor { get; set; }

        [ForeignKey("AvailableSlot")]
        public int SlotId { get; set; }
        public DoctorAvailabilitySlot? AvailableSlot { get; set; }
    }
}
