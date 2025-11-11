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
            public const string Prefix = Rule + "doctor/";
            public const string List = Prefix + "list";
            public const string GetById = Prefix + SingleUser;
        }
    }
}
