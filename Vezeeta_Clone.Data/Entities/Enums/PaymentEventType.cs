namespace Vezeeta_Clone.Data.Entities.Enums
{
    public enum PaymentEventType
    {
        PaymentInitiated,
        PaymentAuthenticated,
        PaymentAuthorized,
        PaymentCaptured,
        PaymentFailed,
        PaymentCancelled,
        RefundInitiated,
        RefundCompleted,
        RefundFailed,
        DisputeOpened,
        DisputeClosed
    }
}
