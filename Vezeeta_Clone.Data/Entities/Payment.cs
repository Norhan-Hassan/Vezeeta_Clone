using System.ComponentModel.DataAnnotations;
using Vezeeta_Clone.Data.Entities.Enums;

namespace Vezeeta_Clone.Data.Entities
{
    public class Payment : BaseEntity
    {

        [Required]
        public PaymentProvider Provider { get; set; }

        [MaxLength(10)]
        public string Currency { get; set; } = "EGP";

        [Range(1.0, double.MaxValue)]
        public decimal Amount { get; set; }
        public string ClientSecret { get; set; }
        public PaymentStatus Status { get; set; }

        public string? ProviderPaymentId { get; set; }
        public string? ProviderTransactionId { get; set; }

        public string? PayerEmail { get; set; }
        public string? PayerName { get; set; }


        public DateTime? PaidAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;


        public string? FailureReason { get; set; }
        public string? FailureCode { get; set; }

        // Idempotency
        public string? IdempotencyKey { get; set; }
        public string? ProviderMetadata { get; set; }


        public ICollection<PaymentEvent> PaymentEvents { get; set; } = new List<PaymentEvent>();

    }
}
