namespace Vezeeta_Clone.Core.Features.Payments.Queries.Results
{
    public class GetPaymentInfoQueryResult
    {
        public int ID { get; set; }
        public string Provider { get; set; }//enum
        public string Currency { get; set; }
        public decimal Amount { get; set; }
        public string ClientSecret { get; set; }
        public string Status { get; set; }//enum
        public string ProviderPaymentId { get; set; }
        public string PayerName { get; set; }
        public string PayerEmail { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
