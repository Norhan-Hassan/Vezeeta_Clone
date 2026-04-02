using System.ComponentModel.DataAnnotations.Schema;
using Vezeeta_Clone.Data.Entities.Enums;

namespace Vezeeta_Clone.Data.Entities
{
    public class OnlineMeeting : BaseEntity
    {
        public string ExternalMeetingId { get; set; }
        public string MeetingUrl { get; set; }
        public OnlineMeetingProvider MeetingProvider { get; set; }
        public DateTime CreatedAt { get; set; }

        [ForeignKey("Appointment")]
        public int? AppointmentId { get; set; }
        public Appointment? Appointment { get; set; }
    }
}
