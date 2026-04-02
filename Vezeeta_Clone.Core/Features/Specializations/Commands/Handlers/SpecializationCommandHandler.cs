using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using Serilog;
using Vezeeta_Clone.Core.Bases;
using Vezeeta_Clone.Core.Features.Specializations.Commands.Models;
using Vezeeta_Clone.Core.Resources;
using Vezeeta_Clone.Data.Entities;
using Vezeeta_Clone.Service.Abstract;

namespace Vezeeta_Clone.Core.Features.Specializations.Commands.Handlers
{
    public class SpecializationCommandHandler : ResponseHandler,
                                            IRequestHandler<CreateSpecializationCommand, Response<string>>,
                                            IRequestHandler<UpdateSpecializationCommand, Response<string>>

    {
        #region Fields
        private readonly ISpecializationService _specializationService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IMapper _mapper;
        #endregion

        #region Constructors
        public SpecializationCommandHandler(IStringLocalizer<SharedResources> localizer,
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

        public async Task<Response<string>> Handle(UpdateSpecializationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var specialization = await _specializationService.GetSpecializationById(request.Id);
                var mappedSpecialization = _mapper.Map(request, specialization);

                //update in database
                var result = await _specializationService.UpdateSpecialization(specialization);
                if (result == "success")
                {
                    return Success<string>(null, _localizer[SharedResourcesKeys.Updated]);
                }
                else
                    return BadRequest<string>(_localizer[SharedResourcesKeys.FailToUpdate]);
            }
            catch (KeyNotFoundException ex)
            {
                Log.Error(ex.Message);
                return NotFound<string>(_localizer[SharedResourcesKeys.NotFound]);
            }

        }


        #endregion
    }
}
