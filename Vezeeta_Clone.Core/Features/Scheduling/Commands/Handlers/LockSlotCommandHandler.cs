using MediatR;
using Microsoft.Extensions.Localization;
using Vezeeta_Clone.Core.Bases;
using Vezeeta_Clone.Core.Features.Scheduling.Commands.Models;
using Vezeeta_Clone.Core.Resources;
using Vezeeta_Clone.Data.Commons;
using Vezeeta_Clone.Service.Abstract;
using Vezeeta_Clone.Service.AppUserAuthServices.Abstract;

namespace Vezeeta_Clone.Core.Features.Scheduling.Commands.Handlers
{
    public class LockSlotCommandHandler : ResponseHandler, IRequestHandler<LockSlotCommand, Response<string>>
    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly ICurrentUserService _currentUserService;
        private readonly ISlotService _slotService;
        #endregion

        #region Constructor
        public LockSlotCommandHandler(IStringLocalizer<SharedResources> localizer,
                                     ISlotService slotService,
                                      ICurrentUserService currentUserService) : base(localizer)
        {
            _localizer = localizer;
            _slotService = slotService;
            _currentUserService = currentUserService;
        }
        #endregion

        #region Functions
        public async Task<Response<string>> Handle(LockSlotCommand request, CancellationToken cancellationToken)
        {
            var roles = await _currentUserService.GetCurrentUserRolesAsync();
            if (!roles.Contains(Roles.Doctor))
            {
                return Unauthorized<string>();
            }
            var doctorId = _currentUserService.GetCurrentUserId();
            var result = await _slotService.LockSlotAsync(request.SlotId, doctorId, request.LockedReason);

            if (result)
            {
                return Success<string>(null, message: _localizer[SharedResourcesKeys.Updated]);
            }
            else
            {
                return BadRequest<string>(_localizer[SharedResourcesKeys.FailToUpdate]);
            }
        }
        #endregion
    }
}
