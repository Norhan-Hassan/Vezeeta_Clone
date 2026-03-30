using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using Vezeeta_Clone.Core.Bases;
using Vezeeta_Clone.Core.Features.Auth.Commands.Models;
using Vezeeta_Clone.Core.Resources;
using Vezeeta_Clone.Data.Results;
using Vezeeta_Clone.Service.Abstract;

namespace Vezeeta_Clone.Core.Features.Auth.Commands.Handlers
{
    public class RefreshTokenCommandHandler : ResponseHandler, IRequestHandler<RefreshTokenCommand, Response<JwtAuthResult>>
    {
        #region Fields
        private readonly IAuthenticationService _authenticationService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructor
        public RefreshTokenCommandHandler(IAuthenticationService authenticationService,
                                    IStringLocalizer<SharedResources> localizer) : base(localizer)
        {
            _localizer = localizer;
            _authenticationService = authenticationService;
        }
        #endregion
        #region Functions
        public async Task<Response<JwtAuthResult>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            JwtAuthResult jwtAuthResult = new JwtAuthResult();
            try
            {
                jwtAuthResult = await _authenticationService.GetRefreshTokenAsync(request.AccessToken, request.RefreshToken);
                return Success<JwtAuthResult>(jwtAuthResult);
            }
            catch (SecurityTokenException ex)
            {
                return Unauthorized<JwtAuthResult>(ex.Message);
            }
        }
        #endregion
    }
}
