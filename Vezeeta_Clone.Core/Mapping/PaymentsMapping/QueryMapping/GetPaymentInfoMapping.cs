using Vezeeta_Clone.Core.Features.Payments.Queries.Results;
using Vezeeta_Clone.Data.Entities;

namespace Vezeeta_Clone.Core.Mapping.PaymentsMapping
{
    public partial class PaymentProfile
    {
        public void GetPaymentInfoMapping()
        {
            CreateMap<Payment, GetPaymentInfoQueryResult>()
                .ForMember(dest => dest.Provider, opt => opt.MapFrom(src => src.Provider.ToString()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

        }
    }
}
