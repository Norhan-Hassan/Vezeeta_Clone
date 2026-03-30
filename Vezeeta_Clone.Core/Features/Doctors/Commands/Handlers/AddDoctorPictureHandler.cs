using MediatR;
using Microsoft.Extensions.Localization;
using Vezeeta_Clone.Core.Bases;
using Vezeeta_Clone.Core.Features.Doctors.Commands.Models;
using Vezeeta_Clone.Core.Resources;
using Vezeeta_Clone.Data.Commons;
using Vezeeta_Clone.Data.Helper;
using Vezeeta_Clone.Service.Abstract;
using Vezeeta_Clone.Service.AppUserAuthServices.Abstract;
using Vezeeta_Clone.Service.ExternalServices.Abstract;

namespace Vezeeta_Clone.Core.Features.Doctors.Commands.Handlers
{
    public class AddDoctorPictureHandler : ResponseHandler, IRequestHandler<AddDoctorPictureCommand, Response<string>>
    {
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IBlobStorageService _blobStorageService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IDoctorService _doctorService;
        private readonly AzureStorageSettings _azureStorageSettings;

        public AddDoctorPictureHandler(IStringLocalizer<SharedResources> localizer,
                                        ICurrentUserService currentUserService,
                                        IDoctorService doctorService,
                                        IBlobStorageService blobStorageService,
                                        AzureStorageSettings azureStorageSettings) : base(localizer)
        {
            _localizer = localizer;
            _currentUserService = currentUserService;
            _doctorService = doctorService;
            _azureStorageSettings = azureStorageSettings;
            _blobStorageService = blobStorageService;
        }

        public async Task<Response<string>> Handle(AddDoctorPictureCommand request, CancellationToken cancellationToken)
        {
            var currentUserRoles = await _currentUserService.GetCurrentUserRolesAsync();
            var currentDoctorId = string.Empty;
            if (currentUserRoles.Contains(Roles.Doctor))
            {
                currentDoctorId = _currentUserService.GetCurrentUserId();
            }
            else
                return Unauthorized<string>();


            var doctor = await _doctorService.GetDoctorByIdAsTrackingAsync(currentDoctorId);
            string blobUrl = string.Empty;
            using (var stream = request.Picture.OpenReadStream())
            {
                if (request.Picture == null || !request.Picture.ContentType.StartsWith("image/"))
                    return BadRequest<string>(_localizer[SharedResourcesKeys.UnSupportedImageType]);

                string contentType = request.Picture.ContentType;

                string blobName = $"{doctor.ApplicationUser.FirstName}_{Guid.NewGuid()}.jpg";

                blobUrl = await _blobStorageService.UploadBlobAsync(
                    _azureStorageSettings.DefaultContainer,
                    blobName,
                    stream,
                    contentType
                );

            }
            var isSaved = await _doctorService.SaveDoctorPicture(doctor, blobUrl);
            if (isSaved)
            {
                return Success<string>(null, _localizer[SharedResourcesKeys.AddSuccess]);
            }
            return BadRequest<string>(_localizer[SharedResourcesKeys.FailToAdd]);
        }
    }
}
