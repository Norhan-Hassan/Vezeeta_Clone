namespace Vezeeta_Clone.Data.AppMetaData
{//{version:apiVersion}
    public static class Router
    {
        public const string Route = "api";
        public const string Version = "v{version:apiVersion}";
        public const string Rule = Route + "/" + Version + "/";
        public const string SingleUser = "{Id:Guid}";
        public const string SingleEntity = "{Id:int}";


        public static class DoctorRouting
        {
            public const string Prefix = Rule + "doctors/";
            public const string List = Prefix;
            public const string GetById = Prefix + SingleUser;
            public const string GetReviews = Prefix + SingleUser + "/reviews";
            public const string GetExamination = Prefix + SingleUser + "/examination-details";
            public const string CompleteInfo = Prefix + "complete-info";
            public const string GetSlots = Prefix + SingleUser + "/available-slots";
            public const string AppointmentsList = Prefix + "appointments/";
            public const string AddPicture = Prefix + "picture/";
        }

        public static class PatientRouting
        {
            public const string Prefix = Rule + "patients/";
            public const string AppointmentsList = Prefix + "appointments/";
            public const string PatientProfile = Prefix;
        }
        public static class ScheduleRouting
        {
            public const string Prefix = Rule + "schedules/";
            public const string AddSchedule = Prefix;
            public const string LockSlot = Prefix + SingleEntity + "/lock-slot";
        }

        public static class ReviewRouting
        {
            public const string Prefix = Rule + "reviews/";
            public const string MakeReview = Prefix;
            public const string UpdateReview = Prefix + SingleEntity;
            public const string DeleteReview = Prefix + SingleEntity;
        }
        public static class AppointmentRouting
        {
            public const string Prefix = Rule + "appointments/";
            public const string BookAppointment = Prefix;
            public const string CancelAppointment = Prefix + SingleEntity + "/cancel";
            public const string CompleteAppointmentBooking = Prefix + SingleEntity;
            public const string GetAppointmentDetails = Prefix + SingleEntity;
        }
        public static class MedicalRecordRouting
        {
            public const string Prefix = Rule + "medical-records/";
            public const string Create = Prefix;
            public const string Diagnosis = Prefix + SingleEntity + "/diagnosis";
            public const string EPrescription = Prefix + SingleEntity + "/e-prescription";
            public const string GenerateMedicalReport = Prefix + "medical-report";

        }
        public static class ClinicRouting
        {
            public const string Prefix = Rule + "clinics/";
            public const string List = Prefix + "list/";
            public const string RegisterClinic = Prefix + "register-to-doctor/";
            public const string AddClinicImages = Prefix + SingleEntity + "/image";
            public const string GetClinicImages = Prefix + SingleEntity + "/images";
        }

        public static class AuthRouting
        {
            public const string Prefix = Rule + "auth/";
            public const string Add = Prefix + "roles";
            public const string Update = Prefix + "roles";
            public const string Delete = Prefix + "roles";
            public const string DoctorRegister = Prefix + "doctor-register";
            public const string PatientRegister = Prefix + "patient-register";
            public const string SignIn = Prefix + "signIn";
            public const string RefreshToken = Prefix + "refresh-token";
            public const string ValidateToken = Prefix + "check-token-validation";
            public const string ChangePassword = Prefix + "change-password";
            public const string ConfirmEmail = Prefix + "confirm-email";
            public const string ResetPassword = Prefix + "request-reset-password";
            public const string CheckResetPassword = Prefix + "check-reset-password";
            public const string ResetPasswordInAction = Prefix + "reset-password";
        }
        public static class SpecializationRouting
        {
            public const string Prefix = Rule + "specializations/";
            public const string Create = Prefix;
            public const string Update = Prefix + SingleEntity;
            public const string List = Prefix;
            public const string SubSpecializations = Prefix + "{SpecializationID:int}/sub-specializations";
        }
        public static class PaymentRouting
        {
            public const string Prefix = Rule + "payments/";
            public const string CreatePaymentIntent = Prefix + "create-payment-intent";
            public const string ConfirmPayment = Prefix + "confirm-payment";
            public const string UpdateAppointmentAfterPayment = Prefix + "update-appointment-after-payment";
            public const string GetPaymentByAppointmentId = Prefix + SingleEntity;
            public const string ProcessCashPayment = Prefix + "process-cash-payment";
            public const string CancellWithRefund = Prefix + SingleEntity + "/cancel-with-refund";
        }
    }
}