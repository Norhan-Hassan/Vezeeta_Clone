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
            public const string List = Prefix + "list";
            public const string GetById = Prefix + SingleUser;
            public const string GetReviews = Prefix + SingleUser + "/reviews";
            public const string GetExamination = Prefix + SingleUser + "/examination-details";
            public const string CompleteInfo = Prefix + "complete-info";
            public const string GetSlots = Prefix + SingleUser + "/available-slots";
            public const string AppointmentsList = Prefix + "appointments/";
        }
        public static class PatientRouting
        {
            public const string Prefix = Rule + "patients/";
            public const string AppointmentsList = Prefix + "appointments/";
        }
        public static class ScheduleRouting
        {
            public const string Prefix = Rule + "schedules/";
            public const string AddSchedule = Prefix + "add";
            public const string LockSlot = Prefix + SingleEntity + "/lock-slot";
        }
        //ReviewRouting

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
            public const string BookAppointment = Prefix + "book";
            public const string CancelAppointment = Prefix + SingleEntity + "/cancel";
            public const string CompleteAppointmentBooking = Prefix + "complete";
        }

        public static class ClinicRouting
        {
            public const string Prefix = Rule + "clinics/";
            public const string List = Prefix + "list";
            public const string RegisterClinic = Prefix + "register-to-doctor";
        }

        public static class AuthRouting
        {
            public const string Prefix = Rule + "auth/";
            public const string Add = Prefix + "role/create";
            public const string Update = Prefix + "role/update";
            public const string Delete = Prefix + "role/delete";
            public const string DoctorRegister = Prefix + "doctor-register";
            public const string PatientRegister = Prefix + "patient-register";
            public const string SignIn = Prefix + "signIn";
            public const string RefreshToken = Prefix + "refresh-token";
            public const string ValidateToken = Prefix + "check-token-validation";
            public const string ChangePassword = Prefix + "change-password";
        }
        public static class SpecializationRouting
        {
            public const string Prefix = Rule + "specializations/";
            public const string Create = Prefix + "create";
            public const string Update = Prefix + "update";
            public const string List = Prefix;
            public const string SubSpecializations = Prefix + "{SpecializationID:int}/sub-specializations";
        }
    }
}
