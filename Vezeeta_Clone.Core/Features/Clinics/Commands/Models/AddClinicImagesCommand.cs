using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vezeeta_Clone.Core.Bases;

namespace Vezeeta_Clone.Core.Features.Clinics.Commands.Models
{
    public class AddClinicImagesCommand : IRequest<Response<string>>
    {
        [FromRoute(Name = "Id")]
        public int ClinicId { get; set; }
        public List<IFormFile> Images { get; set; }
    }
}
