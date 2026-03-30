using MediatR;
using Microsoft.Extensions.Localization;
using Vezeeta_Clone.Core.Bases;
using Vezeeta_Clone.Core.Features.Auth.Queries.Models;
using Vezeeta_Clone.Core.Resources;
using Vezeeta_Clone.Service.Abstract;

namespace Vezeeta_Clone.Core.Features.Auth.Queries.Handlers
{
    public class ConfirmEmailQueryHandler : ResponseHandler,
                                            IRequestHandler<ConfirmEmailQuery, Response<string>>
    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IAuthenticationService _authenticationService;

        #endregion

        #region Constructor
        public ConfirmEmailQueryHandler(IStringLocalizer<SharedResources> localizer, IAuthenticationService authenticationService) : base(localizer)
        {
            _localizer = localizer;
            _authenticationService = authenticationService;
        }
        #endregion

        #region Functions
        public async Task<Response<string>> Handle(ConfirmEmailQuery request, CancellationToken cancellationToken)
        {

            var result = await _authenticationService.ConfirmEmailAsync(request.userEmail, request.code);
            if (result == true)
                return Success<string>(null, message: _localizer[SharedResourcesKeys.EmailConfirmed]);
            return BadRequest<string>(_localizer[SharedResourcesKeys.EmailConfirmationFailed]);

        }
        #endregion
    }
}
