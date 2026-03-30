using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using Vezeeta_Clone.Core.Bases;
using Vezeeta_Clone.Core.Features.MedicalRecords.Commands.Models;
using Vezeeta_Clone.Core.Resources;
using Vezeeta_Clone.Data.Commons;
using Vezeeta_Clone.Data.Entities;
using Vezeeta_Clone.Service.Abstract;
using Vezeeta_Clone.Service.AppUserAuthServices.Abstract;

namespace Vezeeta_Clone.Core.Features.MedicalRecords.Commands.Handlers
{
    public class CreateDiagnosisCommandHandler : ResponseHandler, IRequestHandler<CreateDiagnosisCommand, Response<string>>
    {
        #region Fields
        private readonly IMedicalRecordService _medicalRecordService;
        private readonly IPaymentService _paymentService;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion


        #region Constructor
        public CreateDiagnosisCommandHandler(IStringLocalizer<SharedResources> localizer,
                                                IMapper mapper,
                                                IMedicalRecordService medicalRecordService,
                                                IPaymentService paymentService,
                                                ICurrentUserService currentUserService) : base(localizer)
        {
            _localizer = localizer;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _paymentService = paymentService;
            _medicalRecordService = medicalRecordService;
        }
        #endregion

        #region Functions
        public async Task<Response<string>> Handle(CreateDiagnosisCommand request, CancellationToken cancellationToken)
        {
            var roles = await _currentUserService.GetCurrentUserRolesAsync();
            string doctorId = string.Empty;
            if (roles.Contains(Roles.Doctor))
                doctorId = _currentUserService.GetCurrentUserId();
            else
                return Unauthorized<string>();
            try
            {
                var mappedDiagnosis = _mapper.Map<Diagnosis>(request);
                var createdDiagnosis = await _medicalRecordService.CreateDiagnosisAsync(mappedDiagnosis, request.MedicalRecordId);
                return Created<string>(_localizer[SharedResourcesKeys.AddSuccess]);
            }
            catch (InvalidOperationException ex)
            {
                if (ex.Message.Contains("NoMedicalRecordExist"))
                {
                    return NotFound<string>();
                }
            }
            return BadRequest<string>(_localizer[SharedResourcesKeys.FailToAdd]);
        }
        #endregion
    }
}