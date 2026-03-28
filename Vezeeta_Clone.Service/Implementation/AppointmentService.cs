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
        public async Task<int> BookAppointmentAsync(Appointment appointment, string patientId)
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
                    var slot = await _unitOfWork._availabilitySlotRepo.GetTableNoTracking()
                                                                    .Include(s => s.Availability)
                                                                    .FirstOrDefaultAsync(s => s.ID == appointment.SlotId);

                    if (slot == null)
                        throw new InvalidOperationException("notfound");

                    if (!string.IsNullOrEmpty(slot.LockedReason))
                        throw new InvalidOperationException("Slotislocked");

                    var today = DateOnly.FromDateTime(DateTime.UtcNow);
                    if (slot.Date < today)
                        throw new InvalidOperationException("pastslots");

                    var existingAppointment = await _unitOfWork._appointmentRepo.GetTableNoTracking().AnyAsync(
                                 a => a.SlotId == appointment.SlotId &&
                                 a.Status != AppointmentStatus.Cancelled);

                    if (existingAppointment || slot.IsBooked == true)
                    {
                        throw new InvalidOperationException("alreadybooked");
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
                    return appointment.ID;
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
            return -1;
        }

        public async Task<bool> CompleteAppointmentAsync(Appointment appointment)
        {
            await _unitOfWork._appointmentRepo.UpdateAsync(appointment);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<Appointment> GetAppointmentByIdAsync(int appointmentId)
        {
            var appointment = await _unitOfWork._appointmentRepo.GetTableNoTracking()
                                                                .Include(a => a.Payment).FirstOrDefaultAsync(a => a.ID == appointmentId);
            if (appointment == null)

                return null;
            return appointment;
        }

        public async Task<Appointment> GetAppointmentByIdWithIncludesAsync(int appointmentId)
        {
            var appointment = await _unitOfWork._appointmentRepo.GetTableNoTracking()
                                                                .Include(a => a.Patient)
                                                                    .ThenInclude(p => p.ApplicationUser)
                                                                .Include(a => a.Doctor)
                                                                    .ThenInclude(d => d.ApplicationUser)
                                                                .Include(a => a.Doctor)
                                                                    .ThenInclude(d => d.Clinic)
                                                                .Include(a => a.AvailableSlot)
                                                                .Include(a => a.Payment)

                                                                .FirstOrDefaultAsync(a => a.ID == appointmentId);
            if (appointment == null)

                return null;
            return appointment;
        }

        public IQueryable<Appointment> GetDoctorAppointmentsAsync(string doctorId, AppointmentStatus? appointmentStatus, AvailabilityMethod availabilityMethod, DateOnly? fromDate, DateOnly? toDate)
        {
            var appointments = _unitOfWork._appointmentRepo.GetTableNoTracking()
                                                                .Include(a => a.AvailableSlot)
                                                                .ThenInclude(s => s.Availability)
                                                                .Where(a => a.DoctorId == doctorId)
                                                                .AsQueryable();

            if (fromDate != null && toDate != null && fromDate <= toDate)
            {
                appointments = appointments.Where(a => a.AvailableSlot != null && a.AvailableSlot.Date >= fromDate && a.AvailableSlot.Date <= toDate);
            }
            if (appointmentStatus.HasValue)
            {
                appointments = appointments.Where(a => a.Status == appointmentStatus);
            }
            if (availabilityMethod.HasFlag(AvailabilityMethod.Offline))
            {
                appointments = appointments.Where(a => a.AvailableSlot != null && a.AvailableSlot.Availability.type == AvailabilityMethod.Offline);
            }
            else if (availabilityMethod.HasFlag(AvailabilityMethod.Online))
            {
                appointments = appointments.Where(a => a.AvailableSlot != null && a.AvailableSlot.Availability.type == AvailabilityMethod.Online);
            }

            return appointments;
        }

        public IQueryable<Appointment> GetPatientAppointmentsAsync(string patientId, AppointmentStatus? appointmentStatus)
        {
            var appointments = _unitOfWork._appointmentRepo.GetTableNoTracking()
                                                                .Include(a => a.Doctor)
                                                                .ThenInclude(d => d.ApplicationUser)
                                                                .Include(a => a.AvailableSlot)
                                                                .Where(a => a.PatientId == patientId)
                                                                .AsQueryable();

            if (appointmentStatus.HasValue)
            {
                appointments = appointments.Where(a => a.Status == appointmentStatus);
            }

            return appointments;
        }


    }
}
