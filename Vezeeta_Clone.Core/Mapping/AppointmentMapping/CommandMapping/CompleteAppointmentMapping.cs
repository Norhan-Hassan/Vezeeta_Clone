using Vezeeta_Clone.Core.Features.Appointments.Commands.Models;
using Vezeeta_Clone.Data.Entities;
using Vezeeta_Clone.Data.Entities.Enums;

namespace Vezeeta_Clone.Core.Mapping.AppointmentMapping
{
    public partial class AppointmentProfile
    {
        public void CompleteAppointmentMapping()
        {
            CreateMap<CompleteAppointmentCommand, Appointment>()
                .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.AppointmentId))
                .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => AppointmentStatus.Completed))
                .ForMember(dest => dest.CompletedAt, opt => opt.MapFrom(src => DateTime.UtcNow));


        }

    }
}
