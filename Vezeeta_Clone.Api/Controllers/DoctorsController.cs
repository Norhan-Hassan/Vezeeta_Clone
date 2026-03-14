using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Vezeeta_Clone.Api.Base;
using Vezeeta_Clone.Core.Features.Doctors.Queries.Models;
using Vezeeta_Clone.Data.AppMetaData;

namespace Vezeeta_Clone.Api.Controllers
{
    [ApiVersion("1")]
    public class DoctorsController : AppControllerBase
    {
        [HttpGet(Router.DoctorRouting.List)]
        public async Task<IActionResult> GetDoctorsPaginated([FromQuery] GetDoctorsPaginatedQuery query)
        {
            var response = await _mediator.Send(query);
            return NewResult(response);
        }

        [HttpGet(Router.DoctorRouting.GetById)]
        public async Task<IActionResult> GetDoctorDetails([FromRoute] GetDoctorDetailsQuery query)
        {
            var response = await _mediator.Send(query);
            return NewResult(response);
        }
        [HttpGet(Router.DoctorRouting.GetReviews)]
        public async Task<IActionResult> GetDoctorReviews([FromQuery] GetDoctorReviewsQuery query)
        {
            var response = await _mediator.Send(query);
            return NewResult(response);
        }


        [HttpGet(Router.DoctorRouting.GetExamination)]
        public async Task<IActionResult> GetDoctorExaminationDetails([FromRoute] GetDoctorExaminationDetailsQuery query)
        {
            var response = await _mediator.Send(query);
            return NewResult(response);
        }
    }
}
