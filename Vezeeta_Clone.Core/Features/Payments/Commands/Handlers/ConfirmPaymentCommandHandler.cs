using MediatR;
using Microsoft.Extensions.Localization;
using Vezeeta_Clone.Core.Bases;
using Vezeeta_Clone.Core.Features.Payments.Commands.Models;
using Vezeeta_Clone.Core.Resources;
using Vezeeta_Clone.Service.Abstract;

namespace Vezeeta_Clone.Core.Features.Payments.Commands.Handlers
{
    public class ConfirmPaymentCommandHandler : ResponseHandler, IRequestHandler<ConfirmPaymentCommand, Response<string>>
    {
        private readonly IPaymentService _paymentService;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public ConfirmPaymentCommandHandler(IPaymentService paymentService, IStringLocalizer<SharedResources> localizer)
            : base(localizer)
        {
            _paymentService = paymentService;
            _localizer = localizer;
        }

        public async Task<Response<string>> Handle(ConfirmPaymentCommand request, CancellationToken cancellationToken)
        {
            var result = await _paymentService.ConfirmPaymentAsync(request.PaymentId, request.PaymentMethodId);
            if (!result)
                return BadRequest<string>(_localizer[SharedResourcesKeys.PaymentFailed]);

            return Success<string>(null, message: _localizer[SharedResourcesKeys.PaymentSuccess]);
        }
    }
}
