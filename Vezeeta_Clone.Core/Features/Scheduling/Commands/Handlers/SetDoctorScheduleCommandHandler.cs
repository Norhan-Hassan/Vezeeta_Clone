using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using Vezeeta_Clone.Core.Bases;
using Vezeeta_Clone.Core.Features.Scheduling.Commands.Models;
using Vezeeta_Clone.Core.Resources;
using Vezeeta_Clone.Data.Entities;
using Vezeeta_Clone.Service.Abstract;
using Vezeeta_Clone.Service.AppUserAuthServices.Abstract;

namespace Vezeeta_Clone.Core.Features.Scheduling.Commands.Handlers
{
    public class SetDoctorScheduleCommandHandler : ResponseHandler, IRequestHandler<SetDoctorScheduleCommand, Response<string>>
    {
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly ICurrentUserService _currentUserService;
        private readonly IDoctorAvailabilityService _scheduleService;
        private readonly IMapper _mapper;
        public SetDoctorScheduleCommandHandler(IStringLocalizer<SharedResources> localizer,
                                            IDoctorAvailabilityService scheduleService,
                                            ICurrentUserService currentUserService,
                                            IMapper mapper) : base(localizer)
        {
            _localizer = localizer;
            _scheduleService = scheduleService;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<Response<string>> Handle(SetDoctorScheduleCommand request, CancellationToken cancellationToken)
        {
            var doctorId = _currentUserService.GetCurrentUserId();

            if (doctorId == null)
                return Unauthorized<string>();

            var mappedSchedule = _mapper.Map<DoctorAvailability>(request);

            var response = await _scheduleService.SetDoctorAvailabilityAsync(doctorId, mappedSchedule);

            switch (response)
            {
                case "overlapping":
                    return BadRequest<string>(_localizer[SharedResourcesKeys.ScheduleExist]);
                case "success":
                    return Success<string>(null, _localizer[SharedResourcesKeys.ScheduleAdded]);
                case "fail":
                    return BadRequest<string>(_localizer[SharedResourcesKeys.RequireToHaveClinic]);
                default:
                    return BadRequest<string>(_localizer[SharedResourcesKeys.FailToAdd]);
            }

        }
    }
}
