using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using Serilog;
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
                Log.Error(ex.Message);
                return BadRequest<string>(ex.Message);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return UnprocessableEntity<string>(_localizer[SharedResourcesKeys.FailedToRegister]);
            }

        }
        #endregion
    }
}
