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
    public class DoctorAuthCommandHandler : ResponseHandler,
                                            IRequestHandler<RegisterDoctorCommand, Response<string>>
    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IMapper _mapper;
        private readonly IAuthenticationService _athenticationService;
        #endregion

        #region Constructors
        public DoctorAuthCommandHandler(IStringLocalizer<SharedResources> localizer,
                                        IMapper mapper,
                                        IAuthenticationService athenticationService) : base(localizer)
        {
            _mapper = mapper;
            _athenticationService = athenticationService;
            _localizer = localizer;
        }


        #endregion

        #region
        public async Task<Response<string>> Handle(RegisterDoctorCommand request, CancellationToken cancellationToken)
        {

            var appUser = _mapper.Map<ApplicationUser>(request);
            var doctor = _mapper.Map<Doctor>(request);

            try
            {
                await _athenticationService.RegisterDoctorAsync(doctor, appUser, request.Password);
                return Created("");
            }
            catch (Exception ex)
            {
                return UnprocessableEntity<string>(_localizer[SharedResourcesKeys.FailedToRegister]);
            }

        }
        #endregion
    }
}
