using Vezeeta_Clone.Core.Features.Scheduling.Commands.Models;
using Vezeeta_Clone.Data.Entities;

namespace Vezeeta_Clone.Core.Mapping.ScheduleMapping
{
    public partial class ScheduleProfile
    {
        public void SetDoctorScheduleMapping()
        {
            CreateMap<SetDoctorScheduleCommand, DoctorAvailability>();
            //.ForMember(dest => dest.frequency, opt => opt.MapFrom(src => src.DayOfWeek != null ? ScheduleFrequency.Weekly : ScheduleFrequency.OneTime));

        }
    }
}
