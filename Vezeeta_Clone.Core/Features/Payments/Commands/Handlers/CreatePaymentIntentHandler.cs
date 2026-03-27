using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using Vezeeta_Clone.Core.Bases;
using Vezeeta_Clone.Core.Features.Payments.Commands.Models;
using Vezeeta_Clone.Core.Features.Payments.Commands.Results;
using Vezeeta_Clone.Core.Resources;
using Vezeeta_Clone.Service.Abstract;

namespace Vezeeta_Clone.Core.Features.Payments.Commands.Handlers
{
    public class CreatePaymentIntentCommandHandler : ResponseHandler, IRequestHandler<CreatePaymentIntentCommand, Response<PaymentIntentResponseResult>>
    {
        private readonly IPaymentService _paymentService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public CreatePaymentIntentCommandHandler(IPaymentService paymentService, IMapper mapper,
            IStringLocalizer<SharedResources> localizer) : base(localizer)
        {
            _paymentService = paymentService;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Response<PaymentIntentResponseResult>> Handle(CreatePaymentIntentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var payment = await _paymentService.CreatePaymentIntentAsync(request.AppointmentId, request.Provider);
                if (payment == null)
                    return NotFound<PaymentIntentResponseResult>(_localizer[SharedResourcesKeys.NotFound]);

                return Created(new PaymentIntentResponseResult
                {
                    PaymentId = payment.ID,
                    ClientSecret = payment.ClientSecret,
                    ProviderPaymentId = payment.ProviderPaymentId
                });
            }
            catch (Exception ex)
            {
                return BadRequest<PaymentIntentResponseResult>(ex.Message);
            }
        }
    }
}
