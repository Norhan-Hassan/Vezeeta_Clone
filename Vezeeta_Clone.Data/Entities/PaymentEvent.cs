using System.ComponentModel.DataAnnotations.Schema;
using Vezeeta_Clone.Data.Entities.Enums;

namespace Vezeeta_Clone.Data.Entities
{
    public class PaymentEvent : BaseEntity
    {

        [ForeignKey(nameof(Payment))]
        public int PaymentId { get; set; }
        public Payment Payment { get; set; }
        public PaymentEventType EventType { get; set; }

        public PaymentProvider Provider { get; set; }

        public string? ProviderEventId { get; set; }

        public DateTime ProcessedAt { get; set; } = DateTime.UtcNow;

        // Raw webhook payload from provider
        public string? RawPayload { get; set; }

        // Parsed event data
        public string? EventData { get; set; }

        public int? RetryCount { get; set; } = 0;
    }
}
