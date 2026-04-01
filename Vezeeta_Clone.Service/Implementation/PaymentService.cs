using Microsoft.EntityFrameworkCore;
using Stripe;
using Vezeeta_Clone.Data.Entities;
using Vezeeta_Clone.Data.Entities.Enums;
using Vezeeta_Clone.Data.Helper;
using Vezeeta_Clone.Infrastructure.InfrastructureBases;
using Vezeeta_Clone.Service.Abstract;

namespace Vezeeta_Clone.Service.Implementation
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly StripeSettings _stripeSettings;

        public PaymentService(IUnitOfWork unitOfWork, StripeSettings stripeSettings)
        {
            _unitOfWork = unitOfWork;
            _stripeSettings = stripeSettings;
        }

        public async Task<bool> CheckPaymentAndAppointmentStatusAsync(int appointmentId)
        {
            var status = await _unitOfWork._appointmentRepo.GetTableNoTracking()
                                  .Include(a => a.Payment)
                                 .Where(a => a.ID == appointmentId && a.Status == AppointmentStatus.Confirmed &&
                                                                         a.Payment != null &&
                                                                         a.Payment.Status == PaymentStatus.Paid)
                                 .AnyAsync();
            return status;
        }

        public async Task<Payment> CreatePaymentIntentAsync(int appointmentId, PaymentProvider provider)
        {


            var appointment = await _unitOfWork._appointmentRepo.GetTableNoTracking()
                                                             .Include(a => a.Patient)
                                                             .ThenInclude(p => p.ApplicationUser)
                                                             .Include(a => a.Doctor)
                                                             .ThenInclude(d => d.Clinic)
                                                             .Include(a => a.Payment)
                                                             .FirstOrDefaultAsync(a => a.ID == appointmentId);
            if (appointment?.Payment != null &&
                (appointment.Payment.Status == PaymentStatus.Paid ||
                 appointment.Payment.Status == PaymentStatus.Pending))
            {
                throw new InvalidOperationException("alreadyInPaymentProcess");
            }


            if (appointment?.Doctor?.Clinic == null)
                return null;

            var amount = appointment.Doctor.Clinic.Price;

            if (amount <= 0)
                return null;

            var payment = new Payment
            {
                Provider = provider,
                Amount = amount,
                Currency = "EGP",
                Status = PaymentStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                PayerEmail = appointment.Patient?.ApplicationUser.Email,
                PayerName = appointment.ActualPatientName ?? appointment.Patient?.ApplicationUser.FirstName,
                IdempotencyKey = GenerateIdempotencyKey(appointmentId, provider),
                ProviderMetadata = GenerateProviderMetadata(appointmentId)
            };

            if (provider == PaymentProvider.Stripe)
            {
                try
                {
                    StripeConfiguration.ApiKey = _stripeSettings.SecretKey;
                    var options = new PaymentIntentCreateOptions
                    {
                        Amount = (long)(amount * 100),
                        Currency = "egp",
                        PaymentMethodTypes = new List<string> { "card" },
                        Description = $"Appointment #{appointmentId} - {appointment.Doctor.Clinic.Name}",
                        ReceiptEmail = appointment.Patient?.ApplicationUser.Email,
                        Metadata = new Dictionary<string, string>
                        {
                            { "appointmentId", appointmentId.ToString() },
                            { "patientId", appointment.PatientId }
                        }
                    };

                    var requestOptions = new RequestOptions
                    {
                        IdempotencyKey = payment.IdempotencyKey
                    };

                    var paymentIntentService = new PaymentIntentService();
                    var paymentIntent = await paymentIntentService.CreateAsync(options);

                    payment.ProviderPaymentId = paymentIntent.Id;
                    payment.ClientSecret = paymentIntent.ClientSecret;

                    await LogPaymentEventAsync(
                        payment.ID,
                        PaymentEventType.PaymentIntentCreated,
                        paymentIntent.Id,
                        $"Payment intent created for appointment {appointmentId}"
                    );
                }
                catch (StripeException ex)
                {
                    payment.Status = PaymentStatus.Failed;
                    payment.FailureReason = ex.Message;
                    payment.FailureCode = ex.StripeError?.Code;
                    await _unitOfWork._paymentRepo.AddAsync(payment);
                    await _unitOfWork.SaveChangesAsync();
                    return payment;
                }
            }

            await _unitOfWork._paymentRepo.AddAsync(payment);
            await _unitOfWork.SaveChangesAsync();

            appointment.PaymentId = payment.ID;
            await _unitOfWork._appointmentRepo.UpdateAsync(appointment);
            await _unitOfWork.SaveChangesAsync();

            return payment;
        }

        public async Task<bool> ConfirmPaymentAsync(int paymentId, string paymentMethodId)
        {
            var payment = await _unitOfWork._paymentRepo.GetByIntIdAsync(paymentId);
            if (payment == null || payment.Provider != PaymentProvider.Stripe)
                return false;

            try
            {
                StripeConfiguration.ApiKey = _stripeSettings.SecretKey;
                var options = new PaymentIntentConfirmOptions
                {
                    PaymentMethod = paymentMethodId
                };

                var requestOptions = new RequestOptions
                {
                    IdempotencyKey = $"{payment.IdempotencyKey}_confirm"
                };

                var service = new PaymentIntentService();
                var paymentIntent = await service.ConfirmAsync(payment.ProviderPaymentId, options, requestOptions);

                if (paymentIntent?.Status != "succeeded")
                {
                    payment.Status = PaymentStatus.Failed;
                    payment.FailureReason = $"Stripe confirmation failed: {paymentIntent?.Status}";
                    payment.FailureCode = paymentIntent?.LastPaymentError?.Code;
                    payment.UpdatedAt = DateTime.UtcNow;

                    await _unitOfWork._paymentRepo.UpdateAsync(payment);
                    await _unitOfWork.SaveChangesAsync();

                    await LogPaymentEventAsync(
                                paymentId,
                                PaymentEventType.PaymentIntentFailed,
                                paymentIntent?.Id,
                                $"Confirmation failed: {paymentIntent?.Status}"
                            );
                    return false;
                }

                payment.Status = PaymentStatus.Paid;
                payment.ProviderTransactionId = paymentIntent.Id;
                payment.PaidAt = DateTime.UtcNow;
                payment.UpdatedAt = DateTime.UtcNow;

                await _unitOfWork._paymentRepo.UpdateAsync(payment);
                await _unitOfWork.SaveChangesAsync();

                await LogPaymentEventAsync(
                       paymentId,
                       PaymentEventType.PaymentIntentSucceeded,
                       paymentIntent.Id,
                       "Payment confirmed successfully"
                   );
                return true;
            }
            catch (StripeException ex)
            {
                payment.Status = PaymentStatus.Failed;
                payment.FailureReason = ex.Message;
                payment.FailureCode = ex.StripeError?.Code;
                payment.UpdatedAt = DateTime.UtcNow;
                await _unitOfWork._paymentRepo.UpdateAsync(payment);
                await _unitOfWork.SaveChangesAsync();
                return false;
            }
        }

        public async Task<Appointment> UpdateAppointmentStatusAfterPaymentAsync(int paymentId, bool isPaid)
        {
            var paymentRepo = _unitOfWork._paymentRepo;
            var payment = await paymentRepo.GetByIntIdAsync(paymentId);

            if (payment == null)
                return null;

            var appointmentRepo = _unitOfWork._appointmentRepo;
            var appointment = await appointmentRepo.GetTableNoTracking()
                                                    .Include(a => a.AvailableSlot)
                                                    .FirstOrDefaultAsync(a => a.PaymentId == paymentId);

            if (appointment == null)
                return null;


            using (var transaction = _unitOfWork._appointmentRepo.BeginTransaction())
            {
                try
                {
                    if (isPaid)
                    {

                        await HandleSuccessfulPaymentAsync(appointment, payment);
                    }
                    else
                    {

                        await HandleFailedPaymentAsync(appointment, payment);
                    }


                    await appointmentRepo.UpdateAsync(appointment);
                    await paymentRepo.UpdateAsync(payment);
                    await _unitOfWork.SaveChangesAsync();


                    await transaction.CommitAsync();

                    return appointment;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new InvalidOperationException($"Failed to update appointment after payment: {ex.Message}", ex);
                }
            }
        }

        private async Task HandleSuccessfulPaymentAsync(Appointment appointment, Payment payment)
        {

            if (appointment.Status == AppointmentStatus.Completed)
            {
                appointment.Status = AppointmentStatus.Confirmed;
                appointment.ConfirmedAt = DateTime.UtcNow;
            }
            else if (appointment.Status == AppointmentStatus.Pending)
            {
                throw new InvalidOperationException("PendingAppointment");
            }
            else if (appointment.Status == AppointmentStatus.Confirmed && appointment.Payment.Status == PaymentStatus.Paid)
            {
                throw new InvalidOperationException("alreadyconfirmedandpaid");
            }
            payment.Status = PaymentStatus.Paid;
            payment.PaidAt = DateTime.UtcNow;
            payment.UpdatedAt = DateTime.UtcNow;


            await LogPaymentEventAsync(
                payment.ID,
                PaymentEventType.PaymentIntentSucceeded,
                payment.ProviderTransactionId,
                "Appointment confirmed after successful payment"
            );
        }

        private async Task HandleFailedPaymentAsync(Appointment appointment, Payment payment)
        {

            if (appointment.Status != AppointmentStatus.Confirmed &&
                appointment.Status != AppointmentStatus.Cancelled)
            {
                appointment.Status = AppointmentStatus.Cancelled;
                appointment.CancelledAt = DateTime.UtcNow;
                appointment.CancellationReason = "Payment confirmation failed";

                if (appointment.AvailableSlot != null)
                {
                    appointment.AvailableSlot.IsBooked = false;
                    appointment.AvailableSlot.Status = SlotStatus.Available;


                    await _unitOfWork._availabilitySlotRepo.UpdateAsync(appointment.AvailableSlot);
                }
            }

            payment.Status = PaymentStatus.Failed;
            payment.FailureReason = "Payment confirmation failed";
            payment.UpdatedAt = DateTime.UtcNow;


            await LogPaymentEventAsync(
                payment.ID,
                PaymentEventType.PaymentIntentFailed,
                payment.ProviderPaymentId,
                $"Payment failed for appointment. Failure code: {payment.FailureCode}"
            );
        }


        public async Task<bool> CancelAppointmentWithRefundAsync(int appointmentId, string cancellationReason)
        {
            using (var transaction = _unitOfWork._appointmentRepo.BeginTransaction())
            {
                try
                {
                    var appointment = await _unitOfWork._appointmentRepo.GetTableNoTracking()
                        .Include(a => a.Payment)
                        .Include(a => a.AvailableSlot)
                        .FirstOrDefaultAsync(a => a.ID == appointmentId);

                    if (appointment == null)
                        return false;

                    if (appointment.Status == AppointmentStatus.Completed ||
                        appointment.Status == AppointmentStatus.Cancelled)
                        return false;

                    appointment.Status = AppointmentStatus.Cancelled;
                    appointment.CancelledAt = DateTime.UtcNow;
                    appointment.CancellationReason = cancellationReason;


                    if (appointment.AvailableSlot != null)
                    {
                        appointment.AvailableSlot.IsBooked = false;
                        appointment.AvailableSlot.Status = SlotStatus.Available;
                        await _unitOfWork._availabilitySlotRepo.UpdateAsync(appointment.AvailableSlot);
                    }


                    if (appointment.Payment?.Status == PaymentStatus.Paid)
                    {
                        await HandleRefundAsync(appointment.Payment);
                    }

                    await _unitOfWork._appointmentRepo.UpdateAsync(appointment);
                    await _unitOfWork.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return true;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }


        private async Task HandleRefundAsync(Payment payment)
        {
            if (payment.Provider == PaymentProvider.Stripe &&
                !string.IsNullOrEmpty(payment.ProviderPaymentId))
            {
                try
                {
                    StripeConfiguration.ApiKey = _stripeSettings.SecretKey;
                    var refundService = new RefundService();

                    var refund = await refundService.CreateAsync(new RefundCreateOptions
                    {
                        PaymentIntent = payment.ProviderPaymentId,
                        Amount = (long)(payment.Amount * 100)
                    });

                    payment.Status = PaymentStatus.Refunded;
                    payment.UpdatedAt = DateTime.UtcNow;

                    await LogPaymentEventAsync(
                        payment.ID,
                        PaymentEventType.PaymentIntentFailed,
                        refund.Id,
                        $"Refund initiated: {refund.Id}"
                    );
                }
                catch (StripeException ex)
                {

                    await LogPaymentEventAsync(
                        payment.ID,
                        PaymentEventType.PaymentIntentFailed,
                        null,
                        $"Refund failed: {ex.Message}"
                    );
                }
            }
        }

        public async Task<bool> LogPaymentEventAsync(int paymentId, PaymentEventType eventType,
            string providerEventId = null, string eventData = null)
        {
            var payment = await _unitOfWork._paymentRepo.GetByIntIdAsync(paymentId);
            if (payment == null)
                return false;

            var paymentEvent = new PaymentEvent
            {
                PaymentId = paymentId,
                EventType = eventType,
                Provider = payment.Provider,
                ProviderEventId = providerEventId,
                EventData = eventData,
                ProcessedAt = DateTime.UtcNow
            };

            await _unitOfWork._paymentEventRepo.AddAsync(paymentEvent);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        //inside backend for now 
        private string GenerateIdempotencyKey(int appointmentId, PaymentProvider provider)
        {
            return $"{provider}_{appointmentId}_{DateTime.UtcNow:yyyyMMddHHmmssfff}";
        }

        private string GenerateProviderMetadata(int appointmentId)
        {
            return $"{{\"appointmentId\": {appointmentId}, \"createdAt\": \"{DateTime.UtcNow:O}\"}}";
        }
    }
}