using Microsoft.AspNetCore.Mvc;
using Vezeeta_Clone.Api.Base;
using Vezeeta_Clone.Core.Features.Reviews.Commands.Models;
using Vezeeta_Clone.Data.AppMetaData;

namespace Vezeeta_Clone.Api.Controllers.V1
{
    public class ReviewsController : AppControllerBase
    {
        [HttpPost(Router.ReviewRouting.MakeReview)]
        public async Task<IActionResult> MakeReview([FromBody] MakeReviewCommand command)
        {
            var response = await _mediator.Send(command);
            return NewResult(response);
        }
    }
}
