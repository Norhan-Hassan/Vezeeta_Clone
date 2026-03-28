using MediatR;
using Microsoft.Extensions.Localization;
using Vezeeta_Clone.Core.Bases;
using Vezeeta_Clone.Core.Features.Auth.Commands.Models;
using Vezeeta_Clone.Core.Resources;
using Vezeeta_Clone.Service.Abstract;
using Vezeeta_Clone.Service.BackgroundJobServices.Abstract;
using Vezeeta_Clone.Service.ExternalServices.Abstract;

namespace Vezeeta_Clone.Core.Features.Auth.Commands.Handlers
{
    public class ResetPasswordCommandHandler : ResponseHandler,
                                                IRequestHandler<ResetPasswordCommand, Response<string>>,
                                                IRequestHandler<ResetPasswordInActionCommand, Response<string>>
    {
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IAuthenticationService _autheticationService;
        private readonly IBackgroundJobService _backgroundJobService;
        private readonly IEmailService _emailService;
        public ResetPasswordCommandHandler(IStringLocalizer<SharedResources> localizer,
            IBackgroundJobService backgroundJobService,
            IEmailService emailService,
            IAuthenticationService autheticationService) : base(localizer)
        {
            _localizer = localizer;
            _autheticationService = autheticationService;
            _backgroundJobService = backgroundJobService;
            _emailService = emailService;
        }

        public async Task<Response<string>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var result = await _autheticationService.GetResetPasswordCodeAsync(request.Email);
            if (result != null)
            {
                var variables = new Dictionary<string, string>
                {
                        { "BodyText", _localizer[SharedResourcesKeys.ResetPasswordCodeMessage] },
                        { "ResetCode", result },
                        { "EmailFooter", _localizer[SharedResourcesKeys.EmailFooter] }
                };


                var message = await _emailService.LoadEmailTemplateAsync("ResetPassword.html", variables);

                await _backgroundJobService.EnqueueAsync<IEmailService>(
                        x => x.SendEmail(
                            request.Email,
                            message,
                            _localizer[SharedResourcesKeys.ResetPaswword]
                        )
                    );

                return Success<string>(null, message: _localizer[SharedResourcesKeys.ResetPasswordCodeSentSuccessfully]);
            }
            else
            {
                return NotFound<string>(SharedResourcesKeys.EmailIsNotExist);
            }

        }

        public async Task<Response<string>> Handle(ResetPasswordInActionCommand request, CancellationToken cancellationToken)
        {
            var result = await _autheticationService.ResetPasswordAsync(request.Email, request.Password);
            if (result)
            {
                return Success<string>("");
            }
            else
            {
                return NotFound<string>(SharedResourcesKeys.EmailIsNotExist);
            }
        }
    }
}
