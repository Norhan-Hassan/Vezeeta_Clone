using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Vezeeta_Clone.Api.Base;
using Vezeeta_Clone.Core.Features.Reviews.Commands.Models;
using Vezeeta_Clone.Data.AppMetaData;

namespace Vezeeta_Clone.Api.Controllers.V1
{
    [ApiVersion("1")]
    public class ReviewsController : AppControllerBase
    {
        [HttpPost(Router.ReviewRouting.MakeReview)]
        public async Task<IActionResult> MakeReview([FromBody] MakeReviewCommand command)
        {
            var response = await _mediator.Send(command);
            return NewResult(response);
        }


        [HttpPut(Router.ReviewRouting.UpdateReview)]
        public async Task<IActionResult> UpdateReview([FromRoute] int Id, [FromBody] UpdateReviewCommand command)
        {
            command.Id = Id;
            var response = await _mediator.Send(command);
            return NewResult(response);
        }

        [HttpDelete(Router.ReviewRouting.DeleteReview)]
        public async Task<IActionResult> DeleteReview([FromRoute] DeleteReviewCommand command)
        {
            var response = await _mediator.Send(command);
            return NewResult(response);
        }
    }
}
