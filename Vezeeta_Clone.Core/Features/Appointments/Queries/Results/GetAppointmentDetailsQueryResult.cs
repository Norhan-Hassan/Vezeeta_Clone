namespace Vezeeta_Clone.Core.Features.Appointments.Queries.Results
{
    public class GetAppointmentDetailsQueryResult
    {
        public DateTime BookedAt { get; set; }
        public DateTime? ConfirmedAt { get; set; }
        public DateTime? CancelledAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public string? Notes { get; set; }
        public string? CancellationReason { get; set; }
    }
}
