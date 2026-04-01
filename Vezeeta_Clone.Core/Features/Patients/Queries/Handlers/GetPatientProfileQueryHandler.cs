using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using Vezeeta_Clone.Core.Bases;
using Vezeeta_Clone.Core.Features.Patients.Queries.Models;
using Vezeeta_Clone.Core.Features.Patients.Queries.Results;
using Vezeeta_Clone.Core.Resources;
using Vezeeta_Clone.Data.Commons;
using Vezeeta_Clone.Service.Abstract;
using Vezeeta_Clone.Service.AppUserAuthServices.Abstract;

namespace Vezeeta_Clone.Core.Features.Patients.Queries.Handlers
{
    public class GetPatientProfileQueryHandler : ResponseHandler, IRequestHandler<GetPatientProfileQuery, Response<GetPatientProfileQueryResult>>
    {
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IPatientService _patientService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        public GetPatientProfileQueryHandler(IStringLocalizer<SharedResources> localizer, IMapper mapper, ICurrentUserService currentUserService, IPatientService patientService) : base(localizer)
        {
            _localizer = localizer;
            _patientService = patientService;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<Response<GetPatientProfileQueryResult>> Handle(GetPatientProfileQuery request, CancellationToken cancellationToken)
        {
            var currentUserRoles = await _currentUserService.GetCurrentUserRolesAsync();
            var currentPatientId = string.Empty;
            if (currentUserRoles.Contains(Roles.Patient))
            {
                currentPatientId = _currentUserService.GetCurrentUserId();
            }
            else
            {
                return Unauthorized<GetPatientProfileQueryResult>();
            }
            var patient = await _patientService.GetPatientByIdAsync(currentPatientId);
            if (patient == null)
            {
                return NotFound<GetPatientProfileQueryResult>(_localizer[SharedResourcesKeys.NoData]);
            }
            var mappedPatient = _mapper.Map<GetPatientProfileQueryResult>(patient);
            return Success<GetPatientProfileQueryResult>(mappedPatient);
        }
    }
}
