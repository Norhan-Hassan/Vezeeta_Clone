using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using Vezeeta_Clone.Core.Bases;
using Vezeeta_Clone.Core.Features.Specializations.Commands.Models;
using Vezeeta_Clone.Core.Resources;
using Vezeeta_Clone.Data.Entities;
using Vezeeta_Clone.Service.Abstract;

namespace Vezeeta_Clone.Core.Features.Specializations.Commands.Handlers
{
    public class CreateSpecializationHandler : ResponseHandler,
                                            IRequestHandler<CreateSpecializationCommand, Response<string>>
    {
        #region Fields
        private readonly ISpecializationService _specializationService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IMapper _mapper;
        #endregion

        #region Constructors
        public CreateSpecializationHandler(IStringLocalizer<SharedResources> localizer,
                                            IMapper mapper,
                                            ISpecializationService specializationService) : base(localizer)
        {
            _localizer = localizer;
            _mapper = mapper;
            _specializationService = specializationService;
        }
        #endregion

        #region Functions
        public async Task<Response<string>> Handle(CreateSpecializationCommand request, CancellationToken cancellationToken)
        {
            var mappedSpecialization = _mapper.Map<Specialization>(request);

            //add to database
            var result = await _specializationService.CreateSpecialization(mappedSpecialization);
            if (result == "success")
            {
                return Created<string>("");
            }
            else
                return BadRequest<string>(_localizer[SharedResourcesKeys.FailToAdd]);

        }
        #endregion
    }
}
