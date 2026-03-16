using MediatR;
using Vezeeta_Clone.Core.Bases;
using Vezeeta_Clone.Data.Entities.Enums;

namespace Vezeeta_Clone.Core.Features.Scheduling.Commands.Models
{
    public class SetDoctorScheduleCommand : IRequest<Response<string>>
    {
        public DayOfWeek? DayOfWeek { get; set; }// for weekly recurring schedule
        public DateOnly? Date { get; set; } // for one-time special open days
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }

        public int Duration { get; set; }
        public AvailabilityMethod type { get; set; }
    }
}
