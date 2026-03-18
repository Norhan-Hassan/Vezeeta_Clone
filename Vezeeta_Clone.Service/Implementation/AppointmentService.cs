using Microsoft.EntityFrameworkCore;
using Vezeeta_Clone.Data.Entities;
using Vezeeta_Clone.Data.Entities.Enums;
using Vezeeta_Clone.Infrastructure.Abstract;
using Vezeeta_Clone.Service.Abstract;

namespace Vezeeta_Clone.Service.Implementation
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepo _appointmentRepo;
        private readonly IDoctorAvailabilitySlotRepo _availabilitySlotRepo;
        public AppointmentService(IAppointmentRepo appointmentRepo, IDoctorAvailabilitySlotRepo availabilitySlotRepo)
        {
            _appointmentRepo = appointmentRepo;
            _availabilitySlotRepo = availabilitySlotRepo;
        }
        public async Task<bool> BookAppointmentAsync(Appointment appointment, string patientId)
        {
            if (appointment == null)
                throw new ArgumentNullException(nameof(appointment));

            int maxRetries = 3;
            int retryCount = 0;

            while (retryCount < maxRetries)
            {
                try
                {
                    var slot = await _availabilitySlotRepo.GetByIntIdAsync(appointment.SlotId);

                    if (slot == null)
                        throw new InvalidOperationException("Slot not found");

                    if (!string.IsNullOrEmpty(slot.LockedReason))
                        throw new InvalidOperationException("Slot is locked Due to  " + slot.LockedReason);

                    var today = DateOnly.FromDateTime(DateTime.UtcNow);
                    if (slot.Date < today)
                        throw new InvalidOperationException("Cannot book past slots");

                    var existingAppointment = await _appointmentRepo.GetTableNoTracking().AnyAsync(
                                 a => a.SlotId == appointment.SlotId &&
                                 a.Status != AppointmentStatus.Cancelled);

                    if (existingAppointment || slot.IsBooked == true)
                    {
                        throw new InvalidOperationException("This slot is already booked");
                    }


                    slot.IsBooked = true;
                    slot.Status = SlotStatus.Booked;
                    appointment.PatientId = patientId;

                    // appointment.Status = AppointmentStatus.Pending;
                    // appointment.BookedAt = DateTime.UtcNow;

                    await _appointmentRepo.AddAsync(appointment);
                    await _availabilitySlotRepo.UpdateAsync(slot);

                    try
                    {
                        await _appointmentRepo.SaveChangesAsync();
                        await _availabilitySlotRepo.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        throw new InvalidOperationException("This slot was just booked by another user, try another slot");
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
    }
}
