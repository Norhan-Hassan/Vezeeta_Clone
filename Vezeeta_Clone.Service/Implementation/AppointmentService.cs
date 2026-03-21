using Microsoft.EntityFrameworkCore;
using Vezeeta_Clone.Data.Entities;
using Vezeeta_Clone.Data.Entities.Enums;
using Vezeeta_Clone.Infrastructure.InfrastructureBases;
using Vezeeta_Clone.Service.Abstract;

namespace Vezeeta_Clone.Service.Implementation
{
    public class AppointmentService : IAppointmentService
    {

        private readonly IUnitOfWork _unitOfWork;
        public AppointmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> BookAppointmentAsync(Appointment appointment, string patientId)
        {
            if (appointment == null)
                throw new ArgumentNullException(nameof(appointment));

            int maxRetries = 3;
            int retryCount = 0;

            while (retryCount < maxRetries)
            {
                using var transaction = _unitOfWork._appointmentRepo.BeginTransaction();
                try
                {
                    // var slot = await _unitOfWork._availabilitySlotRepo
                    //     .GetByIntIdAsync(appointment.SlotId);

                    var slot = await _unitOfWork._availabilitySlotRepo.GetTableNoTracking().Include(s => s.Availability)
                        .FirstOrDefaultAsync(s => s.ID == appointment.SlotId);

                    if (slot == null)
                        throw new InvalidOperationException("Slot not found");

                    if (!string.IsNullOrEmpty(slot.LockedReason))
                        throw new InvalidOperationException("Slot is locked Due to  " + slot.LockedReason);

                    var today = DateOnly.FromDateTime(DateTime.UtcNow);
                    if (slot.Date < today)
                        throw new InvalidOperationException("Cannot book past slots");

                    var existingAppointment = await _unitOfWork._appointmentRepo.GetTableNoTracking().AnyAsync(
                                 a => a.SlotId == appointment.SlotId &&
                                 a.Status != AppointmentStatus.Cancelled);

                    if (existingAppointment || slot.IsBooked == true)
                    {
                        throw new InvalidOperationException("This slot is already booked");
                    }


                    slot.IsBooked = true;
                    slot.Status = SlotStatus.Booked;
                    appointment.PatientId = patientId;
                    appointment.DoctorId = slot.Availability.DoctorId;


                    await _unitOfWork._appointmentRepo.AddAsync(appointment);
                    await _unitOfWork._availabilitySlotRepo.UpdateAsync(slot);

                    try
                    {
                        await _unitOfWork.SaveChangesAsync();
                        transaction.Commit();
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        retryCount++;
                        if (retryCount >= maxRetries)
                            throw new InvalidOperationException("This slot was just booked by another user, try another slot");

                        await Task.Delay(50);
                        transaction.Rollback();
                    }
                    return true;
                }
                catch (InvalidOperationException ex)
                {
                    //log the exception
                    throw;
                }
                catch (Exception ex)
                {
                    //log the exception
                    throw;
                }
            }
            return false;
        }

        public async Task<bool> CompleteAppointmentAsync(Appointment appointment)
        {
            //appointment.Status = AppointmentStatus.Completed;

            await _unitOfWork._appointmentRepo.UpdateAsync(appointment);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<Appointment> GetAppointmentByIdAsync(int appointmentId)
        {
            var appointment = await _unitOfWork._appointmentRepo.GetByIntIdAsync(appointmentId);
            if (appointment == null)
                throw new InvalidOperationException("Appointment not found");
            return appointment;
        }
    }
}
