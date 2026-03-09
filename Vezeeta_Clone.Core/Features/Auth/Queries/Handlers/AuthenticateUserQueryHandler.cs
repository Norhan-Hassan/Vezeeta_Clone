using MediatR;
using Microsoft.Extensions.Localization;
using Vezeeta_Clone.Core.Bases;
using Vezeeta_Clone.Core.Features.Auth.Queries.Models;
using Vezeeta_Clone.Core.Resources;
using Vezeeta_Clone.Service.Abstract;

namespace Vezeeta_Clone.Core.Features.Auth.Queries.Handlers
{
    public class AuthenticateUserQueryHandler : ResponseHandler, IRequestHandler<AuthenticateUserQuery, Response<string>>
    {
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IAuthenticationService _authenticationService;

        public AuthenticateUserQueryHandler(IStringLocalizer<SharedResources> localizer,
                                    IAuthenticationService authenticationService) : base(localizer)
        {
            _authenticationService = authenticationService;
            _localizer = localizer;
        }


        public async Task<Response<string>> Handle(AuthenticateUserQuery request, CancellationToken cancellationToken)
        {
            var isValid = await _authenticationService.ValidateJwtToken(request.AccessToken);
            return isValid
                ? Success<string>(entity: request.AccessToken, message: _localizer[SharedResourcesKeys.ValidToken])
                : BadRequest<string>(_localizer[SharedResourcesKeys.InValidToken]);
        }
    }
}
