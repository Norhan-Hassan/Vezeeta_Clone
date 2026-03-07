using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using Vezeeta_Clone.Core.Bases;
using Vezeeta_Clone.Core.Features.Auth.Commands.Models;
using Vezeeta_Clone.Core.Resources;
using Vezeeta_Clone.Data.Entities;
using Vezeeta_Clone.Service.Abstract;

namespace Vezeeta_Clone.Core.Features.Auth.Commands.Handlers
{
    public class PatientAuthCommandHandler : ResponseHandler,
                                             IRequestHandler<RegisterPatientCommand, Response<string>>
    {
        private readonly IMapper _mapper;
        private readonly IAuthenticationService _authenticationService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        public PatientAuthCommandHandler(IStringLocalizer<SharedResources> localizer,
                                        IAuthenticationService authenticationService,
                                        IMapper mapper) : base(localizer)
        {
            _mapper = mapper;
            _authenticationService = authenticationService;
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
                return Created("");
            }
            catch (Exception ex)
            {
                return UnprocessableEntity<string>(_localizer[SharedResourcesKeys.FailedToRegister]);
            }

        }
    }
}
