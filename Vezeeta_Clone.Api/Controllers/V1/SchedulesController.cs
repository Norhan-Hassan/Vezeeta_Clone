using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Vezeeta_Clone.Api.Base;
using Vezeeta_Clone.Core.Features.Scheduling.Commands.Models;
using Vezeeta_Clone.Data.AppMetaData;
using Vezeeta_Clone.Data.Commons;

namespace Vezeeta_Clone.Api.Controllers.V1
{
    [ApiVersion("1.0")]
    [Authorize(Roles = Roles.Doctor)]
    public class SchedulesController : AppControllerBase
    {
        [HttpPost(Router.ScheduleRouting.AddSchedule)]
        [SwaggerOperation(Summary = "Set doctor availability schedule", Description = "Create doctor availability with time slots. Supports weekly recurring schedules and one-time special dates")]
        public async Task<IActionResult> AddSchedule([FromBody] SetDoctorScheduleCommand command)
        {
            var response = await _mediator.Send(command);
            return NewResult(response);
        }


        [HttpPut(Router.ScheduleRouting.LockSlot)]
        [SwaggerOperation(Summary = "Lock appointment slot", Description = "Lock a specific appointment slot to prevent patient bookings")]
        public async Task<IActionResult> LockSlot([FromRoute] int Id, [FromBody] LockSlotCommand command)
        {
            command.SlotId = Id;
            var response = await _mediator.Send(command);
            return NewResult(response);
        }
    }
}
