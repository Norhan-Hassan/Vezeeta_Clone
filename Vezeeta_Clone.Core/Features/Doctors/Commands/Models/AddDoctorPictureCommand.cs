using MediatR;
using Microsoft.AspNetCore.Http;
using Vezeeta_Clone.Core.Bases;

namespace Vezeeta_Clone.Core.Features.Doctors.Commands.Models
{
    public class AddDoctorPictureCommand : IRequest<Response<string>>
    {
        public IFormFile Picture { get; set; }
    }
}
