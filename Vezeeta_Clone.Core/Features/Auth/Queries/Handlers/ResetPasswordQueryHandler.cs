using MediatR;
using Microsoft.Extensions.Localization;
using Vezeeta_Clone.Core.Bases;
using Vezeeta_Clone.Core.Features.Auth.Queries.Models;
using Vezeeta_Clone.Core.Resources;
using Vezeeta_Clone.Service.Abstract;

namespace Vezeeta_Clone.Core.Features.Auth.Queries.Handlers
{
    public class ResetPasswordQueryHandler : ResponseHandler, IRequestHandler<ResetPasswordQuery, Response<string>>
    {
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IAuthenticationService _authenticationService;
        public ResetPasswordQueryHandler(IStringLocalizer<SharedResources> localizer, IAuthenticationService authenticationService) : base(localizer)
        {
            _localizer = localizer;
            _authenticationService = authenticationService;
        }

        public async Task<Response<string>> Handle(ResetPasswordQuery request, CancellationToken cancellationToken)
        {
            var result = await _authenticationService.CheckResetPasswordCodeAsync(request.Email, request.Code);
            if (result == "validcode")
            {
                return Success<string>("");
            }
            else if (result == "codeExpired")
                return BadRequest<string>(_localizer[SharedResourcesKeys.InvalidResetCode]);
            else
                return BadRequest<string>(_localizer[SharedResourcesKeys.EmailIsNotExist]);

        }
    }
}
