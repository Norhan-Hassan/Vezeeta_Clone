using Microsoft.EntityFrameworkCore;
using Vezeeta_Clone.Data.Entities;
using Vezeeta_Clone.Data.Entities.Enums;
using Vezeeta_Clone.Infrastructure.InfrastructureBases;
using Vezeeta_Clone.Service.Abstract;

namespace Vezeeta_Clone.Service.Implementation
{
    public class ReviewService : IReviewService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ReviewService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> MakeReviewAsync(Review review, string patientId)
        {
            var doctor = await _unitOfWork._doctorRepo.GetByStringIdAsync(review.DoctorId);
            if (doctor == null)
            {
                throw new NullReferenceException("Doctor not found");
            }



            var patientReview = await _unitOfWork._reviewRepo.GetTableNoTracking()
                                                              .AnyAsync(r => r.PatientId == patientId && r.DoctorId == review.DoctorId);

            var patientDoctorAppointment = await _unitOfWork._appointmentRepo.GetTableNoTracking()
                                                                    .AnyAsync(a => a.DoctorId == review.DoctorId && a.PatientId == patientId && a.Status == AppointmentStatus.Completed);
            if (patientReview)
            {
                throw new InvalidOperationException("AlreadyReviewed");
            }

            if (!patientDoctorAppointment)
            {
                throw new InvalidOperationException("DoesnotHaveAppointment");
            }
            review.PatientId = patientId;
            await _unitOfWork._reviewRepo.AddAsync(review);

            var result = await _unitOfWork.SaveChangesAsync();
            if (result >= 0)
            {
                return true;
            }
            return false;
        }
    }
}
