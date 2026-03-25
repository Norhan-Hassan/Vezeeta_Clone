using Vezeeta_Clone.Core.Features.Appointments.Queries.Results;
using Vezeeta_Clone.Data.Entities;

namespace Vezeeta_Clone.Core.Mapping.AppointmentMapping
{
    public partial class AppointmentProfile
    {
        public void GetAppointmentDetailesMapping()
        {
            CreateMap<Appointment, GetAppointmentDetailsQueryResult>();

        }
    }
}
