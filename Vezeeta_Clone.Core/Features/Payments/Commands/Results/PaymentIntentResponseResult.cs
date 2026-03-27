namespace Vezeeta_Clone.Core.Features.Payments.Commands.Results
{
    public class PaymentIntentResponseResult
    {
        public int PaymentId { get; set; }
        public string ClientSecret { get; set; }
        public string ProviderPaymentId { get; set; }
    }
}
