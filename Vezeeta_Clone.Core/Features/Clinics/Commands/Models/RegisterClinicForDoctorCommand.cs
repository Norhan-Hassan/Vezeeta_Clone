using MediatR;
using Vezeeta_Clone.Core.Bases;
using Vezeeta_Clone.Core.Features.Clinics.Shared;

namespace Vezeeta_Clone.Core.Features.Clinics.Commands.Models
{
    public class RegisterClinicForDoctorCommand : IRequest<Response<string>>
    {
        public string ClinicName { get; set; }
        public string Address { get; set; }
        public int RegionId { get; set; }
        public LocationDto ClinicLocation { get; set; }
        public string PhoneNumber { get; set; }
        public int WaitingTimeInMinutes { get; set; }
        public decimal Price { get; set; }
    }

}
