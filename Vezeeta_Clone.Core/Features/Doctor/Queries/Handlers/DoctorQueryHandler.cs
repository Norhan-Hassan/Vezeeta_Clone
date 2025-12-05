using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using Vezeeta_Clone.Core.Bases;
using Vezeeta_Clone.Core.Features.Doctor.Queries.Models;
using Vezeeta_Clone.Core.Features.Doctor.Queries.Results;
using Vezeeta_Clone.Core.Resources;
using Vezeeta_Clone.Service.Abstract;

namespace Vezeeta_Clone.Core.Features.Doctor.Queries.Handlers
{
    public class DoctorQueryHandler : ResponseHandler, IRequestHandler<GetDoctorByIdQuery, Response<GetDoctorByIdResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IDoctorService _doctorService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        public DoctorQueryHandler(IMapper mapper, IDoctorService doctorService, IStringLocalizer<SharedResources> localizer)
        {
            _mapper = mapper;
            _doctorService = doctorService;
            _localizer = localizer;
        }

        public async Task<Response<GetDoctorByIdResponse>> Handle(GetDoctorByIdQuery request, CancellationToken cancellationToken)
        {
            var doctor = await _doctorService.GetDoctorByIDAsync(request.Id);
            if (doctor == null) { return NotFound<GetDoctorByIdResponse>(_localizer[SharedResourcesKeys.NotFound]); }
            else
            {
                var result = _mapper.Map<GetDoctorByIdResponse>(doctor);
                return Success(result);
            }
        }
    }
}
