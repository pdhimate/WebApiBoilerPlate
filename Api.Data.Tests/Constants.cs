namespace Api.Data.IntegrationTests
{
    public class Constants
    {
        public class AppConfig
        {
            public const string DatabaseConnectionKey = "AppDatabaseConnection";
        }

        public class DefaultUsers
        {
            public static string DefaultTestUserPassword { get { return "abcd12#$"; } }
        }
    }
}
