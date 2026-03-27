using AutoMapper;

namespace Vezeeta_Clone.Core.Mapping.PaymentsMapping
{
    public partial class PaymentProfile : Profile
    {
        public PaymentProfile()
        {
            GetPaymentInfoMapping();
        }
    }
}
