using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using Vezeeta_Clone.Core.Bases;
using Vezeeta_Clone.Core.Features.Doctors.Queries.Models;
using Vezeeta_Clone.Core.Features.Doctors.Queries.Results;
using Vezeeta_Clone.Core.Resources;
using Vezeeta_Clone.Core.Wrappers;
using Vezeeta_Clone.Service.Abstract;
using Vezeeta_Clone.Service.AppUserAuthServices.Abstract;

namespace Vezeeta_Clone.Core.Features.Doctors.Queries.Handlers
{
    public class GetDoctorAvailableSlotsQueryHandler : ResponseHandler, IRequestHandler<GetDoctorAvailableSlotsQuery, Response<List<DoctorAvailableSlotQueryResult>>>
    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly ICurrentUserService _currentUserService;
        private readonly ISlotService _slotService;
        private readonly IMapper _mapper;
        #endregion

        #region Constructor
        public GetDoctorAvailableSlotsQueryHandler(IStringLocalizer<SharedResources> localizer,
                                    ICurrentUserService currentUserService,
                                               IMapper mapper,
                                               ISlotService slotService) : base(localizer)
        {
            _mapper = mapper;
            _currentUserService = currentUserService;
            _localizer = localizer;
            _slotService = slotService;
        }
        #endregion

        #region Functions

        public async Task<Response<List<DoctorAvailableSlotQueryResult>>> Handle(GetDoctorAvailableSlotsQuery request, CancellationToken cancellationToken)
        {

            var doctorSlots = await _slotService.GetDoctorAvailableSlotsAsync(request.Id);
            if (doctorSlots is null)
            {
                return NotFound<List<DoctorAvailableSlotQueryResult>>(_localizer[SharedResourcesKeys.NoData]);
            }

            var groupedSlots = doctorSlots
                .GroupBy(s => s.Date)
                .Select(g => new DoctorAvailableSlotQueryResult
                {
                    Date = g.Key.ToString("d-M"),
                    DayName = LocalizableDate.GetDayName(_localizer, g.Key),
                    StartTimes = g.Select(s => new SlotInfo
                    {
                        StartTime = s.StartTime.ToShortTimeString(),
                        Id = s.ID
                    }).ToList()
                }).ToList();

            return Success(groupedSlots, _localizer[SharedResourcesKeys.Success]);
        }
        #endregion
    }
}
