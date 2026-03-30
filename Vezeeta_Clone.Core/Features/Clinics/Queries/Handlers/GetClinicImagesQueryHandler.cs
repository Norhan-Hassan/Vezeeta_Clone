using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using Vezeeta_Clone.Core.Bases;
using Vezeeta_Clone.Core.Features.Clinics.Queries.Models;
using Vezeeta_Clone.Core.Features.Clinics.Queries.Results;
using Vezeeta_Clone.Core.Resources;
using Vezeeta_Clone.Service.Abstract;

namespace Vezeeta_Clone.Core.Features.Clinics.Queries.Handlers
{
    public class GetClinicImagesQueryHandler : ResponseHandler, IRequestHandler<GetClinicImagesQuery, Response<List<GetClinicImagesQueryResult>>>
    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IClinicService _clinicService;
        private readonly IMapper _mapper;
        #endregion

        #region Constructor
        public GetClinicImagesQueryHandler(IStringLocalizer<SharedResources> localizer,
                                            IClinicService clinicService,
                                            IMapper mapper) : base(localizer)
        {
            _localizer = localizer;
            _clinicService = clinicService;
            _mapper = mapper;
        }
        #endregion

        #region Functions
        public async Task<Response<List<GetClinicImagesQueryResult>>> Handle(GetClinicImagesQuery request, CancellationToken cancellationToken)
        {
            var clinic = await _clinicService.GetClinicByIdAsync(request.ClinicId);
            if (clinic == null)
            {
                return NotFound<List<GetClinicImagesQueryResult>>();
            }

            var clinicImages = await _clinicService.GetClinicImagesByClinicIdAsync(request.ClinicId);
            if (clinicImages == null || !clinicImages.Any())
            {
                return BadRequest<List<GetClinicImagesQueryResult>>(_localizer[SharedResourcesKeys.NoData]);
            }


            var mappedClinicImages = _mapper.Map<List<GetClinicImagesQueryResult>>(clinicImages);

            return Success<List<GetClinicImagesQueryResult>>(mappedClinicImages);
        }
        #endregion
    }
}
