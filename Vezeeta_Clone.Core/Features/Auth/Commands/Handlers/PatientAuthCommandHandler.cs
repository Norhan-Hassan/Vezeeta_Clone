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
    public class PatientAuthCommandHandler : ResponseHandler,
                                             IRequestHandler<RegisterPatientCommand, Response<string>>
    {
        private readonly IMapper _mapper;
        private readonly IAuthenticationService _authenticationService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IEmailService _emailService;
        private readonly IBackgroundJobService _backgroundJobService;

        public PatientAuthCommandHandler(IStringLocalizer<SharedResources> localizer,
                                        IAuthenticationService authenticationService,
                                        IEmailService emailService,
                                            IBackgroundJobService backgroundJobService,
                                        IMapper mapper) : base(localizer)
        {
            _mapper = mapper;
            _authenticationService = authenticationService;
            _backgroundJobService = backgroundJobService;
            _emailService = emailService;
            _localizer = localizer;
        }
        public async Task<Response<string>> Handle(RegisterPatientCommand request, CancellationToken cancellationToken)
        {
            //mapping
            var appUser = _mapper.Map<ApplicationUser>(request);
            var patient = _mapper.Map<Patient>(request);
            try
            {
                await _authenticationService.RegisterPatientAsync(patient, appUser, request.Password);

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
                          _localizer[SharedResourcesKeys.AppointmentConfirmation]
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
    }
}
