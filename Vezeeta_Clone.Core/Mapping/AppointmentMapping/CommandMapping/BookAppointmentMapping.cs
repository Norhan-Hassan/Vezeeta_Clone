using Vezeeta_Clone.Core.Features.Appointments.Commands.Models;
using Vezeeta_Clone.Data.Entities;
using Vezeeta_Clone.Data.Entities.Enums;

namespace Vezeeta_Clone.Core.Mapping.AppointmentMapping
{
    public partial class AppointmentProfile
    {
        public void BookAppointmentMapping()
        {
            CreateMap<BookAppointmentCommand, Appointment>()
                .ForMember(dest => dest.BookedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => AppointmentStatus.Pending))
                .ForMember(dest => dest.SlotId, opt => opt.MapFrom(src => src.SlotId));
        }
    }
}
