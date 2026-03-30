using MediatR;
using Microsoft.AspNetCore.Mvc;
using Vezeeta_Clone.Core.Bases;
using Vezeeta_Clone.Core.Features.Clinics.Queries.Results;

namespace Vezeeta_Clone.Core.Features.Clinics.Queries.Models
{
    public class GetClinicImagesQuery : IRequest<Response<List<GetClinicImagesQueryResult>>>
    {
        [FromRoute(Name = "Id")]
        public int ClinicId { get; set; }
    }
}
