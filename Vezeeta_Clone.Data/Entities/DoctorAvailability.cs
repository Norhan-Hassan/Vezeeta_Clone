using System.ComponentModel.DataAnnotations.Schema;
using Vezeeta_Clone.Data.Entities.Enums;

namespace Vezeeta_Clone.Data.Entities
{
    public class DoctorAvailability : BaseEntity
    {
        /// <summary>
        /// if consider dayof week then it is  weekly recurring schedule
        /// 
        /// else if consider Specific Date for one-time special open days
        /// 
        /// else consider both as null ,then generate for all days of week and for all dates in the future until end date
        /// </summary>
        public DayOfWeek? DayOfWeek { get; set; }// for weekly recurring schedule
        public DateOnly? Date { get; set; } // for one-time special open days


        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }

        public int Duration { get; set; }
        public AvailabilityMethod type { get; set; }

        [ForeignKey("Doctor")]
        public string DoctorId { get; set; }
        public Doctor Doctor { get; set; }

        public ICollection<DoctorAvailabilitySlot>? AvailableSlots { get; set; } = new HashSet<DoctorAvailabilitySlot>();


    }
}
