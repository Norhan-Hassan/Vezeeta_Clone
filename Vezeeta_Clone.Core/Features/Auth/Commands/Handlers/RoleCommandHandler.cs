using MediatR;
using Microsoft.Extensions.Localization;
using Vezeeta_Clone.Core.Bases;
using Vezeeta_Clone.Core.Features.Auth.Commands.Models;
using Vezeeta_Clone.Core.Resources;
using Vezeeta_Clone.Service.Abstract;

namespace Vezeeta_Clone.Core.Features.Auth.Commands.Handlers
{
    public class RoleCommandHandler : ResponseHandler,
                                    IRequestHandler<AddRoleCommand, Response<string>>,
                                    IRequestHandler<UpdateRoleCommand, Response<string>>,
                                    IRequestHandler<DeleteRoleCommand, Response<string>>
    {
        #region Fields
        private readonly IAutherizationService _autherizationService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion


        #region Constructors
        public RoleCommandHandler(IAutherizationService autherizationService,
                                    IStringLocalizer<SharedResources> localizer) : base(localizer)
        {
            _autherizationService = autherizationService;
            _localizer = localizer;

        }
        #endregion


        #region Functions
        public async Task<Response<string>> Handle(AddRoleCommand request, CancellationToken cancellationToken)
        {
            var result = await _autherizationService.AddRoleAync(request.RoleName);
            if (result == true)
            {
                return Success<string>("");
            }
            else
            {
                return BadRequest<string>(_localizer[SharedResourcesKeys.FailToAdd]);
            }
        }

        public async Task<Response<string>> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            var isUpdated = await _autherizationService.UpdateRoleAync(request.Id, request.RoleName);
            if (isUpdated)
            {
                return Updated<string>("");
            }
            else
            {
                return NotFound<string>(_localizer[SharedResourcesKeys.NotFound]);
            }
        }

        public async Task<Response<string>> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            var isDeleted = await _autherizationService.DeleteRoleAync(request.Id);
            if (isDeleted)
            {
                return Deleted<string>("");
            }
            else
            {
                return NotFound<string>(_localizer[SharedResourcesKeys.NotFound]);
            }
        }
        #endregion

    }
}
