using Microsoft.EntityFrameworkCore;
using Vezeeta_Clone.Data.Entities.Enums;
using Vezeeta_Clone.Infrastructure.InfrastructureBases;
using Vezeeta_Clone.Service.Abstract;

namespace Vezeeta_Clone.Service.Implementation
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        public PaymentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> CheckPaymentAndAppointmentStatusAsync(int appointmentId)
        {
            var status = await _unitOfWork._appointmentRepo.GetTableNoTracking()
                                  .Include(a => a.Payment)
                                 .Where(a => a.ID == appointmentId && a.Status == AppointmentStatus.Completed)//&&
                                                                                                              //a.Payment != null &&
                                                                                                              //  a.Payment.Status == PaymentStatus.Paid)
                                 .AnyAsync();
            return status;
        }
    }
}
