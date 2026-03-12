namespace Vezeeta_Clone.Data.AppMetaData
{
    public static class Router
    {
        public const string Route = "api";
        public const string Version = "v1";
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
