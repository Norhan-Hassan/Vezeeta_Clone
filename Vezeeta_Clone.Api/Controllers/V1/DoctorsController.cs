using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Vezeeta_Clone.Api.Base;
using Vezeeta_Clone.Core.Features.Doctors.Commands.Models;
using Vezeeta_Clone.Core.Features.Doctors.Queries.Models;
using Vezeeta_Clone.Data.AppMetaData;
using Vezeeta_Clone.Data.Commons;

namespace Vezeeta_Clone.Api.Controllers.V1
{
    [ApiVersion("1")]
    public class DoctorsController : AppControllerBase
    {
        [HttpGet(Router.DoctorRouting.List)]
        [SwaggerOperation(Summary = "Search doctors", Description = "Get paginated list of doctors with filtering and sorting options")]

        public async Task<IActionResult> GetDoctorsPaginated([FromQuery] GetDoctorsPaginatedQuery query)
        {
            var response = await _mediator.Send(query);
            return NewResult(response);
        }

        [HttpGet(Router.DoctorRouting.GetById)]
        [SwaggerOperation(Summary = "Retrieve doctor", Description = "Retrieve One doctor details by id")]
        public async Task<IActionResult> GetDoctorDetails([FromRoute] GetDoctorDetailsQuery query)
        {
            var response = await _mediator.Send(query);
            return NewResult(response);
        }

        [HttpGet(Router.DoctorRouting.GetReviews)]
        [SwaggerOperation(Summary = "Get doctor reviews", Description = "Get paginated list of reviews for a specific doctor")]
        public async Task<IActionResult> GetDoctorReviews([FromQuery] GetDoctorReviewsQuery query)
        {
            var response = await _mediator.Send(query);
            return NewResult(response);
        }


        [HttpGet(Router.DoctorRouting.GetExamination)]
        [SwaggerOperation(Summary = "Get doctor examination details", Description = "Get detailed information about a specific examination offered by a doctor")]
        public async Task<IActionResult> GetDoctorExaminationDetails([FromRoute] GetDoctorExaminationDetailsQuery query)
        {
            var response = await _mediator.Send(query);
            return NewResult(response);
        }

        [HttpGet(Router.DoctorRouting.GetSlots)]
        [SwaggerOperation(Summary = "Get doctor available slots", Description = "Get a list of available appointment slots for doctor grouped by date")]
        public async Task<IActionResult> GetDoctorAvailableSlots([FromRoute] GetDoctorAvailableSlotsQuery query)
        {
            var response = await _mediator.Send(query);
            return NewResult(response);
        }

        [Authorize(Roles = Roles.Doctor)]
        [HttpGet(Router.DoctorRouting.AppointmentsList)]
        [SwaggerOperation(Summary = "Appoinments of Doctor ", Description = "Get paginated list of appointments of the current Doctor")]

        public async Task<IActionResult> GetDoctorsPaginated([FromQuery] GetDoctorAppointmentsQuery query)
        {
            var response = await _mediator.Send(query);
            return NewResult(response);
        }

        [HttpPost(Router.DoctorRouting.CompleteInfo)]
        [SwaggerOperation(Summary = "Complete doctor information", Description = "Complete the registration process for a doctor by providing additional required information to be able to register a clinic")]
        public async Task<IActionResult> CompleteDoctorInfo([FromBody] CompleteDoctorInfoCommand command)
        {
            var response = await _mediator.Send(command);
            return NewResult(response);
        }

        [Authorize(Roles = Roles.Doctor)]
        [HttpPost(Router.DoctorRouting.AddPicture)]
        [SwaggerOperation(Summary = "Add Doctor picture ", Description = "Add Picture to current registered doctor ")]
        public async Task<IActionResult> AddDoctorpicture([FromForm] AddDoctorPictureCommand command)
        {
            var response = await _mediator.Send(command);
            return NewResult(response);
        }


    }
}
