using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using Vezeeta_Clone.Core.Bases;
using Vezeeta_Clone.Core.Features.Doctors.Commands.Models;
using Vezeeta_Clone.Core.Resources;
using Vezeeta_Clone.Service.Abstract;
using Vezeeta_Clone.Service.AppUserAuthServices.Abstract;

namespace Vezeeta_Clone.Core.Features.Doctors.Commands.Handlers
{
    public class DoctorCommandHandler : ResponseHandler, IRequestHandler<CompleteDoctorInfoCommand, Response<string>>
    {
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly ICurrentUserService _currentUserService;
        private readonly IDoctorService _doctorService;
        private readonly IMapper _mapper;
        public DoctorCommandHandler(IStringLocalizer<SharedResources> localizer,
                                      IMapper mapper,
                                      ICurrentUserService currentUserService,
                                      IDoctorService doctor) : base(localizer)
        {
            _mapper = mapper;
            _localizer = localizer;
            _currentUserService = currentUserService;
            _doctorService = doctor;
        }

        public async Task<Response<string>> Handle(CompleteDoctorInfoCommand request, CancellationToken cancellationToken)
        {
            //get doctor
            var doctorId = _currentUserService.GetCurrentUserId();
            var doctor = await _doctorService.GetDoctorByIdWithoutIncludesAsync(doctorId);

            var result = await _doctorService.CompleteDoctorInfoAsync(doctor, request.SubSpecializations, request.Description);
            if (result)
            {
                return Success<string>(null, message: _localizer[SharedResourcesKeys.DoctorCompleteInfo]);
            }
            else
            {
                return BadRequest<string>(_localizer[SharedResourcesKeys.FailedToCompleteDoctorInfo]);
            }
        }
    }
}
//Doctor info completed successfully
//Failed to complete doctor info