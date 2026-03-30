using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using Vezeeta_Clone.Core.Bases;
using Vezeeta_Clone.Core.Features.Clinics.Commands.Models;
using Vezeeta_Clone.Core.Resources;
using Vezeeta_Clone.Data.Commons;
using Vezeeta_Clone.Data.Entities;
using Vezeeta_Clone.Data.Helper;
using Vezeeta_Clone.Service.Abstract;
using Vezeeta_Clone.Service.AppUserAuthServices.Abstract;
using Vezeeta_Clone.Service.ExternalServices.Abstract;

namespace Vezeeta_Clone.Core.Features.Clinics.Commands.Handlers
{
    public class AddClinicImagesCommandHandler : ResponseHandler, IRequestHandler<AddClinicImagesCommand, Response<string>>
    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IBlobStorageService _blobStorageService;
        private readonly AzureStorageSettings _azureStorageSettings;
        private readonly IClinicService _clinicService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        #endregion

        #region Constructor
        public AddClinicImagesCommandHandler(IStringLocalizer<SharedResources> localizer,
                                             IBlobStorageService blobStorageService,
                                             IClinicService clinicService,
                                             ICurrentUserService currentUserService,
                                             IMapper mapper,
                                             AzureStorageSettings azureStorageSettings) : base(localizer)
        {
            _localizer = localizer;
            _mapper = mapper;
            _blobStorageService = blobStorageService;
            _currentUserService = currentUserService;
            _clinicService = clinicService;
            _azureStorageSettings = azureStorageSettings;
        }
        #endregion

        #region Functions
        public async Task<Response<string>> Handle(AddClinicImagesCommand request, CancellationToken cancellationToken)
        {
            var clinic = await _clinicService.GetClinicByIdAsync(request.ClinicId);
            if (clinic == null)
            {
                return NotFound<string>(_localizer[SharedResourcesKeys.NotFound]);
            }


            var currentUserRoles = await _currentUserService.GetCurrentUserRolesAsync();
            var currentDoctorId = string.Empty;
            if (currentUserRoles.Contains(Roles.Doctor))
            {
                currentDoctorId = _currentUserService.GetCurrentUserId();
            }
            if (currentDoctorId != clinic.DoctorId)
            {
                return Unauthorized<string>();
            }
            var uploadedUrls = new List<string>();
            try
            {
                for (int i = 0; i < request.Images.Count; i++)
                {
                    var image = request.Images[i];

                    if (image.Length == 0)
                        continue;

                    using (var stream = image.OpenReadStream())
                    {
                        if (image == null || !image.ContentType.StartsWith("image/"))
                            return BadRequest<string>(_localizer[SharedResourcesKeys.UnSupportedImageType]);

                        string contentType = image.ContentType;

                        string blobName = $"{clinic.Name}_{i}_{Guid.NewGuid()}.jpg";

                        string blobUrl = await _blobStorageService.UploadBlobAsync(
                            _azureStorageSettings.DefaultContainer,
                            blobName,
                            stream,
                            contentType
                        );

                        uploadedUrls.Add(blobUrl);
                    }
                }

                var clinicImages = _mapper.Map<List<ClinicImage>>(uploadedUrls);

                await _clinicService.SaveClinicImages(clinic.DoctorId, clinicImages);

                return Success(string.Join(",", uploadedUrls));
            }
            catch (Exception ex)
            {
                return BadRequest<string>(ex.Message);
            }
        }
        #endregion
    }
}
