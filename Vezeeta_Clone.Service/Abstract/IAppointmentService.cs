using Vezeeta_Clone.Data.Entities;

namespace Vezeeta_Clone.Service.Abstract
{
    public interface IAppointmentService
    {
        Task<bool> BookAppointmentAsync(Appointment appointment, string patientId);

    }

}
