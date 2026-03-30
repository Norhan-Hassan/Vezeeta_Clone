using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using Vezeeta_Clone.Core.Bases;
using Vezeeta_Clone.Core.Features.Specializations.Queries.Models;
using Vezeeta_Clone.Core.Features.Specializations.Queries.Results;
using Vezeeta_Clone.Core.Resources;
using Vezeeta_Clone.Service.Abstract;

namespace Vezeeta_Clone.Core.Features.Specializations.Queries.Handlers
{
    public class SpecializationQueryHandler : ResponseHandler,
                                              IRequestHandler<GetSpecializationsQuery, Response<List<GetSpecializationsQueryResult>>>,
                                             IRequestHandler<GetSubSpecializationBySpecIDQuery, Response<List<GetSubSpecializationBySpecIDQueryResult>>>


    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IMapper _mapper;
        private readonly ISpecializationService _specializationService;
        #endregion

        #region Constructor
        public SpecializationQueryHandler(IStringLocalizer<SharedResources> localizer,
                                          IMapper mapper,
                                          ISpecializationService specializationService) : base(localizer)
        {
            _localizer = localizer;
            _mapper = mapper;
            _specializationService = specializationService;
        }
        #endregion

        #region Functions
        public async Task<Response<List<GetSpecializationsQueryResult>>> Handle(GetSpecializationsQuery request, CancellationToken cancellationToken)
        {
            var specializations = await _specializationService.GetSpecializationsAsync();
            if (specializations != null)
            {
                var mappedSpecializations = _mapper.Map<List<GetSpecializationsQueryResult>>(specializations);
                return Success(mappedSpecializations);
            }
            else
            {
                return NotFound<List<GetSpecializationsQueryResult>>(_localizer[SharedResourcesKeys.NotFound]);
            }
        }
        public async Task<Response<List<GetSubSpecializationBySpecIDQueryResult>>> Handle(GetSubSpecializationBySpecIDQuery request, CancellationToken cancellationToken)
        {
            var subspecializations = await _specializationService.GetSubSpecializationBySpecIDAsync(request.SpecializationID);
            if (subspecializations != null)
            {
                var mappedSubSpecialization = _mapper.Map<List<GetSubSpecializationBySpecIDQueryResult>>(subspecializations);
                return Success(mappedSubSpecialization);
            }
            else
            {
                return NotFound<List<GetSubSpecializationBySpecIDQueryResult>>(_localizer[SharedResourcesKeys.NotFound]);
            }
        }
        #endregion
    }
}
