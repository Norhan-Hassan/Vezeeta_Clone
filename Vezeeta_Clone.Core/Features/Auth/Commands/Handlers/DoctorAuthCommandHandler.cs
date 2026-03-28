using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using Vezeeta_Clone.Core.Bases;
using Vezeeta_Clone.Core.Features.Auth.Commands.Models;
using Vezeeta_Clone.Core.Resources;
using Vezeeta_Clone.Data.Entities;
using Vezeeta_Clone.Service.Abstract;
using Vezeeta_Clone.Service.BackgroundJobServices.Abstract;
using Vezeeta_Clone.Service.ExternalServices.Abstract;

namespace Vezeeta_Clone.Core.Features.Auth.Commands.Handlers
{
    public class DoctorAuthCommandHandler : ResponseHandler,
                                            IRequestHandler<RegisterDoctorCommand, Response<string>>
    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IBackgroundJobService _backgroundJobService;
        #endregion

        #region Constructors
        public DoctorAuthCommandHandler(IStringLocalizer<SharedResources> localizer,
                                        IMapper mapper,
                                        IEmailService emailService,
                                 IAuthenticationService authenticationService,
                                 IBackgroundJobService backgroundJobService) : base(localizer)
        {
            _mapper = mapper;
            _authenticationService = authenticationService;
            _backgroundJobService = backgroundJobService;
            _localizer = localizer;
            _emailService = emailService;
        }


        #endregion

        #region Functions
        public async Task<Response<string>> Handle(RegisterDoctorCommand request, CancellationToken cancellationToken)
        {

            var appUser = _mapper.Map<ApplicationUser>(request);
            var doctor = _mapper.Map<Doctor>(request);

            try
            {
                await _authenticationService.RegisterDoctorAsync(doctor, appUser, request.Password);

                //email confirmation
                var returnedUrl = await _authenticationService.GetEmailConfirmationUrlAsync(appUser);

                var variables = new Dictionary<string, string>
                {
                        { "BodyText", _localizer[SharedResourcesKeys.ConfirmEmailBody] },
                        { "ConfirmUrl", returnedUrl },
                        { "ButtonText", _localizer[SharedResourcesKeys.ConfirmEmailButton] },
                        { "EmailFooter", _localizer[SharedResourcesKeys.EmailFooter] }
                };

                var message = await _emailService.LoadEmailTemplateAsync("EmailConfirmation.html", variables);

                await _backgroundJobService.EnqueueAsync<IEmailService>(
                        x => x.SendEmail(
                            appUser.Email,
                            message,
                            _localizer[SharedResourcesKeys.EmailConfirmation]
                        )
                    );
                return Created("");

            }
            catch (FileNotFoundException ex)
            {
                return BadRequest<string>(ex.Message);
            }
            catch (Exception ex)
            {
                return UnprocessableEntity<string>(_localizer[SharedResourcesKeys.FailedToRegister]);
            }

        }
        #endregion
    }
}
//var message = $@"
//                    <table style='width:100%; font-family:Arial, sans-serif; background-color:#f4f4f4; padding:40px 0;'>
//                      <tr>
//                        <td align='center'>
//                          <!-- Main Container -->
//                          <table style='width:600px; background-color:#ffffff; border-radius:12px; box-shadow:0 4px 20px rgba(0,0,0,0.1); overflow:hidden; text-align:center;'>

//                            <!-- Logo Section -->
//                            <tr>
//                              <td style='padding:30px 0;'>
//                                <!-- Logo -->
//                                <img src='https://res.cloudinary.com/ddtcswz77/image/upload/v1774491046/Logo_lotycd.png' alt='Logo' width='100' style='display:block; margin:0 auto;'/>
//                              </td>
//                            </tr>

//                            <!-- App Name -->
//                            <tr>
//                              <td style='padding-bottom:20px;'>
//                                <h2 style='margin:0; font-size:24px; color:#333333;'>{_localizer["AppName"] ?? "Manisik"}</h2>
//                              </td>
//                            </tr>

//                            <!-- Body Section -->
//                            <tr>
//                              <td style='padding:0 30px 30px 30px; color:#333333; font-size:16px; line-height:1.6;'>
//                                <p style='font-size:18px; font-weight:500; margin-bottom:20px;'>{_localizer[SharedResourcesKeys.ConfirmEmailBody]}</p>

//                                <!-- Confirm Button -->
//                                <a href='{returnedUrl}' 
//                                   style='display:inline-block; background-color:#007bff; color:#ffffff; 
//                                          padding:14px 28px; text-decoration:none; border-radius:8px; font-weight:bold;
//                                          font-size:16px; box-shadow:0 4px 12px rgba(0,0,0,0.15);'>
//                                   {_localizer[SharedResourcesKeys.ConfirmEmailButton]}
//                                </a>
//                              </td>
//                            </tr>

//                            <!-- Footer Section -->
//                            <tr>
//                              <td style='padding:20px; text-align:center; font-size:12px; color:#888888; background-color:#f9f9f9;'>
//                                If you did not request this email, you can safely ignore it.
//                              </td>
//                            </tr>

//                          </table>
//                        </td>
//                      </tr>
//                    </table>";