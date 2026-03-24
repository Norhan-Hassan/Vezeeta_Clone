using MediatR;
using System.Text.Json.Serialization;
using Vezeeta_Clone.Core.Bases;

namespace Vezeeta_Clone.Core.Features.Scheduling.Commands.Models
{
    public class LockSlotCommand : IRequest<Response<string>>
    {
        [JsonIgnore]
        public int SlotId { get; set; }
        public string LockedReason { get; set; }
    }
}
