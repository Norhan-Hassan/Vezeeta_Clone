using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using Vezeeta_Clone.Core.Bases;
using Vezeeta_Clone.Core.Features.Clinics.Commands.Models;
using Vezeeta_Clone.Core.Resources;
using Vezeeta_Clone.Data.Commons;
using Vezeeta_Clone.Data.Entities;
using Vezeeta_Clone.Service.Abstract;
using Vezeeta_Clone.Service.AppUserAuthServices.Abstract;

namespace Vezeeta_Clone.Core.Features.Clinics.Commands.Handlers
{
    public class ClinicCommandHandler : ResponseHandler, IRequestHandler<RegisterClinicForDoctorCommand, Response<string>>
    {
        private readonly IClinicService _clinicService;
        private readonly IDoctorService _doctorService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IMapper _mapper;
        public ClinicCommandHandler(IClinicService clinicService, ICurrentUserService currentUserService, IDoctorService doctorService, IStringLocalizer<SharedResources> localizer, IMapper mapper) : base(localizer)
        {
            _localizer = localizer;
            _clinicService = clinicService;
            _currentUserService = currentUserService;
            _doctorService = doctorService;
            _mapper = mapper;

        }
        public async Task<Response<string>> Handle(RegisterClinicForDoctorCommand request, CancellationToken cancellationToken)
        {
            //mapping
            var mappedClinic = _mapper.Map<Clinic>(request);

            try
            {
                var roles = await _currentUserService.GetCurrentUserRolesAsync();
                foreach (var role in roles)
                {
                    if (role == Roles.Doctor)
                    {
                        var doctorId = _currentUserService.GetCurrentUserId();
                        if (await _clinicService.IsClinicExist(doctorId))
                        {
                            return BadRequest<string>(_localizer[SharedResourcesKeys.OneClinicOnly]);
                        }
                        await _clinicService.RegisterClinicToDoctor(mappedClinic, doctorId);
                        return Success<string>("");
                    }
                }
                return Unauthorized<string>();
            }
            catch (Exception ex)
            {
                return BadRequest<string>(ex.Message);
            }


        }
    }
}
