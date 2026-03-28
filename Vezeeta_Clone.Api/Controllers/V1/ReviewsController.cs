using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Vezeeta_Clone.Api.Base;
using Vezeeta_Clone.Core.Features.Reviews.Commands.Models;
using Vezeeta_Clone.Data.AppMetaData;
using Vezeeta_Clone.Data.Commons;

namespace Vezeeta_Clone.Api.Controllers.V1
{
    [ApiVersion("1")]
    public class ReviewsController : AppControllerBase
    {
        [HttpPost(Router.ReviewRouting.MakeReview)]
        [Authorize(Roles = Roles.Patient)]
        [SwaggerOperation(Summary = "Create doctor review", Description = "Create a new review for a doctor with rating (0-5) and optional comment")]
        public async Task<IActionResult> MakeReview([FromBody] MakeReviewCommand command)
        {
            var response = await _mediator.Send(command);
            return NewResult(response);
        }

        [HttpPut(Router.ReviewRouting.UpdateReview)]
        [Authorize(Roles = Roles.Patient)]
        [SwaggerOperation(Summary = "Update doctor review", Description = "Update an existing review rating and comment")]
        public async Task<IActionResult> UpdateReview([FromRoute] int Id, [FromBody] UpdateReviewCommand command)
        {
            command.Id = Id;
            var response = await _mediator.Send(command);
            return NewResult(response);
        }

        [HttpDelete(Router.ReviewRouting.DeleteReview)]
        [Authorize(Roles = Roles.Patient)]
        [SwaggerOperation(Summary = "Delete doctor review", Description = "Delete an existing review")]
        public async Task<IActionResult> DeleteReview([FromRoute] DeleteReviewCommand command)
        {
            var response = await _mediator.Send(command);
            return NewResult(response);
        }
    }
}
