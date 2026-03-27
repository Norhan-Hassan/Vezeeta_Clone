using MediatR;
using Microsoft.Extensions.Localization;
using Vezeeta_Clone.Core.Bases;
using Vezeeta_Clone.Core.Features.Auth.Commands.Models;
using Vezeeta_Clone.Core.Resources;
using Vezeeta_Clone.Service.Abstract;
using Vezeeta_Clone.Service.BackgroundJobServices.Abstract;

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
                var message = $@"
                    <table style='width:100%; font-family:Arial, sans-serif; background-color:#f4f4f4; padding:40px 0;'>
                      <tr>
                        <td align='center'>
                          <table style='width:600px; background-color:#ffffff; border-radius:12px; box-shadow:0 4px 20px rgba(0,0,0,0.1); overflow:hidden; text-align:center;'>
        
                            <tr>
                              <td style='padding:30px 0;'>
                                <!-- Logo -->
                                <img src='https://res.cloudinary.com/ddtcswz77/image/upload/v1774491046/Logo_lotycd.png' alt='Logo' width='100' style='display:block; margin:0 auto;'/>
                              </td>
                            </tr>


                            <tr>
                              <td style='padding-bottom:20px;'>
                                <h2 style='margin:0; font-size:24px; color:#333333;'>{_localizer[SharedResourcesKeys.AppName]}</h2>
                              </td>
                            </tr>

   
                            <tr>
                              <td style='padding:0 30px 30px 30px; color:#333333; font-size:16px; line-height:1.6;'>
                                <p style='font-size:18px; font-weight:500; margin-bottom:20px;'>{_localizer[SharedResourcesKeys.ResetPasswordCodeMessage]}</p>     
                                                {result}
                              </td>
                            </tr>

                            <tr>
                              <td style='padding:20px; text-align:center; font-size:12px; color:#888888; background-color:#f9f9f9;'>
                                If you did not request this email, you can safely ignore it.
                              </td>
                            </tr>

                          </table>
                        </td>
                      </tr>
                    </table>";

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
