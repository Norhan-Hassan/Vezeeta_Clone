using Microsoft.AspNetCore.Mvc;
using Vezeeta_Clone.Api.Base;
using Vezeeta_Clone.Core.Features.Scheduling.Commands.Models;
using Vezeeta_Clone.Data.AppMetaData;

namespace Vezeeta_Clone.Api.Controllers.V1
{
    public class SchedulesController : AppControllerBase
    {
        [HttpPost(Router.ScheduleRouting.AddSchedule)]
        public async Task<IActionResult> AddSchedule([FromBody] SetDoctorScheduleCommand command)
        {
            var response = await _mediator.Send(command);
            return NewResult(response);
        }


        [HttpPut(Router.ScheduleRouting.LockSlot)]
        public async Task<IActionResult> LockSlot([FromRoute] int Id, [FromBody] LockSlotCommand command)
        {
            command.SlotId = Id;
            var response = await _mediator.Send(command);
            return NewResult(response);
        }
    }
}
