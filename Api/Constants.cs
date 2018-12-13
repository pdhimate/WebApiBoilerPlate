namespace Api
{
    public static class Constants
    {
        internal static class ApiRoutes
        {
            public static string BaseApiPath { get { return "/api"; } }

            /// <summary>
            /// Api path for generating bearer access tokens
            /// </summary>
            internal static string TokenEndpointPath { get { return BaseApiPath + "/token"; } }

            /// <summary>
            /// 
            /// </summary>
            internal static string ExternalAuthorizationEndpointPath { get { return BaseApiPath + "/AccountService/ExternalLogin"; } }
        }

        internal class RequestHeaders
        {
            internal static string TwoFactorOtpKey { get { return "X-OTP"; } }
        }

        /// <summary>
        /// SMS provider
        /// </summary>
        internal class Twilio
        {
            internal static string AccountSid { get { return "YourTwilioSid"; } }

            internal static string AuthToken { get { return "YourTwilioToken"; } }

            /// <summary>
            /// The phone number of the Twilio account. This is the number the SMSes would be sent from.
            /// </summary>
            internal static string PhoneNumber { get { return "YourTwiliono"; } }

            /// <summary>
            /// The verified phone number for the trial account. 
            /// When using trial account SMS can only be sent to the verified numbers.
            /// </summary>
            internal static string TrialAccountPhoneNumber { get { return "YourTwilioTrialNo"; } }
        }

        /// <summary>
        /// Email provider
        /// </summary>
        internal class SendGrid
        {
            internal static string ApiKey { get { return "YourSendGridAPIKey"; } }

            internal static string Id { get { return "YourSendGridId"; } }
        }

        public class ExceptionCustomDataKeys
        {
            public static string ErrorReferenceKey { get { return "ErrorReference"; } }
            public static string LoggingFailureKey { get { return "LoggingFailure"; } }
        }

        internal class WebConfig
        {
            /// <summary>
            /// The appsettings key pointing to the url of the GUI website.
            /// </summary>
            public static string WebsiteBaseUrlKey { get { return "WebsiteBaseUrl"; } }

            /// <summary>
            /// The appsettings key for the ConfirmEmailPage's path on the GUI website.
            /// </summary>
            public static string ConfirmEmailPagePathKey { get { return "ConfirmEmailPagePath"; } }

            /// <summary>
            /// The appsettings key for the ResetPasswordPage's path on the GUI website.
            /// </summary>
            public static string ResetPasswordPagePathKey { get { return "ResetPasswordPagePath"; } }

        }

    }
}