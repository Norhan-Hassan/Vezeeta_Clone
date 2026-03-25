using AutoMapper;

namespace Vezeeta_Clone.Core.Mapping.AppointmentMapping
{
    public partial class AppointmentProfile : Profile
    {
        public AppointmentProfile()
        {
            BookAppointmentMapping();
            CompleteAppointmentMapping();
            GetAppointmentDetailesMapping();
        }
    }
}
