using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Vezeeta_Clone.Api.Base;
using Vezeeta_Clone.Core.Features.MedicalRecords.Commands.Models;
using Vezeeta_Clone.Data.AppMetaData;
using Vezeeta_Clone.Data.Commons;

namespace Vezeeta_Clone.Api.Controllers.V1
{
    [ApiVersion("1.0")]
    public class MedicalRecordsController : AppControllerBase
    {
        [HttpPost(Router.MedicalRecordRouting.Create)]
        [Authorize(Roles = Roles.Doctor)]
        [SwaggerOperation(Summary = "Create Medical Record", Description = "Create Medical record betweeen patient and current signed in doctor , patient appointment booking should have been completed and payment is paid ")]
        public async Task<IActionResult> CreateMedicalRecord([FromBody] CreateMedicalRecordCommand command)
        {
            var response = await _mediator.Send(command);
            return NewResult(response);
        }

        [HttpPost(Router.MedicalRecordRouting.Diagnosis)]
        [Authorize(Roles = Roles.Doctor)]
        [SwaggerOperation(Summary = "Create Diagnosis", Description = "Create new diagnosis in the medical record between doctor and patient")]
        public async Task<IActionResult> CreateDiagnosisRecord([FromRoute] int Id, [FromBody] CreateDiagnosisCommand command)
        {
            command.MedicalRecordId = Id;
            var response = await _mediator.Send(command);
            return NewResult(response);
        }

        //CreateEPrescription
        [HttpPost(Router.MedicalRecordRouting.EPrescription)]
        [Authorize(Roles = Roles.Doctor)]
        [SwaggerOperation(Summary = "Create EPrescription", Description = "Create new E-Prescription in the medical record between doctor and patient")]
        public async Task<IActionResult> CreateEPrescription([FromRoute] int Id, [FromBody] CreateEPrescriptionCommand command)
        {
            command.MedicalRecordId = Id;
            var response = await _mediator.Send(command);
            return NewResult(response);
        }
    }
}
