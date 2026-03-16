using AutoMapper;

namespace Vezeeta_Clone.Core.Mapping.ScheduleMapping
{
    public partial class ScheduleProfile : Profile
    {
        public ScheduleProfile()
        {
            SetDoctorScheduleMapping();
        }
    }
}
