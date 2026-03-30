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
    public class CreateMedicalRecordCommandHandler : ResponseHandler, IRequestHandler<CreateMedicalRecordCommand, Response<string>>
    {
        #region Fields
        private readonly IMedicalRecordService _medicalRecordService;
        private readonly IPaymentService _paymentService;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructor
        public CreateMedicalRecordCommandHandler(IStringLocalizer<SharedResources> localizer,
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

        public async Task<Response<string>> Handle(CreateMedicalRecordCommand request, CancellationToken cancellationToken)
        {
            var roles = await _currentUserService.GetCurrentUserRolesAsync();
            string doctorId = string.Empty;
            if (roles.Contains(Roles.Doctor))
                doctorId = _currentUserService.GetCurrentUserId();
            else
                return Unauthorized<string>();
            try
            {
                var AppointmentPaymentStatus = await _paymentService.CheckPaymentAndAppointmentStatusAsync(request.AppointmentId);
                if (!AppointmentPaymentStatus)
                    return NotFound<string>(_localizer[SharedResourcesKeys.AppointmentStatusPaymentStatus]);

                var mappedMedicalRecord = _mapper.Map<MedicalRecord>(request);
                var medicalRecord = await _medicalRecordService.CreateMedicalRecordAsync(mappedMedicalRecord, doctorId, request.PatientId);
                return Created<string>(_localizer[SharedResourcesKeys.AddSuccess]);
            }
            catch (InvalidOperationException ex)
            {
                if (ex.Message.Contains("there_is_no_appointment_between"))
                    return NotFound<string>(_localizer[SharedResourcesKeys.NoAppointmentBetween]);

                if (ex.Message.Contains("Failed_to_record_doctor-patient_visit"))
                    return NotFound<string>(_localizer[SharedResourcesKeys.NoPatientVisit]);

                else if (ex.Message.Contains("alreadyExists"))
                    return BadRequest<string>(_localizer[SharedResourcesKeys.RecordAlreadyExists]);

            }
            return BadRequest<string>(_localizer[SharedResourcesKeys.FailToAdd]);

        }
        #endregion
    }
}
