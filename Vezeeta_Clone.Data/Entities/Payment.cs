using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        // Provider-specific IDs
        public string? ProviderPaymentId { get; set; }
        public string? ProviderTransactionId { get; set; }

        // Payer information
        public string? PayerEmail { get; set; }
        public string? PayerName { get; set; }

        // Timestamps
        public DateTime? PaidAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Error handling
        public string? FailureReason { get; set; }
        public string? FailureCode { get; set; }

        // Idempotency
        public string? IdempotencyKey { get; set; }

        // Provider-specific data (JSON for flexibility)
        public string? ProviderMetadata { get; set; }

        // Relationships
        public ICollection<PaymentEvent> PaymentEvents { get; set; } = new List<PaymentEvent>();

        [ForeignKey(nameof(Appointment))]
        public int AppointmentId { get; set; }
        public Appointment Appointment { get; set; }

    }
}
