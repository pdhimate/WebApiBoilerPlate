using System.Linq;
using System.Net.Http;

namespace Api.Auth
{
    public static class OtpHelper
    {
        /// <summary>
        /// Looks for a custom header named “X-OTP” <see cref="Constants.RequestHeaders.TwoFactorOtpKey"/> in the HTTP request.
        /// Validates the OTP, if present.
        /// </summary>
        /// <param name="httpRequestMessage"></param>
        /// <param name="code"></param>
        /// <returns>True, if OTP is valid.</returns>
        public static bool HasValidTotp(this HttpRequestMessage httpRequestMessage, string code)
        {
            if (httpRequestMessage.Headers.Contains(Constants.RequestHeaders.TwoFactorOtpKey))
            {
                string otp = httpRequestMessage.Headers.GetValues(Constants.RequestHeaders.TwoFactorOtpKey).First();

                // We need to check the passcode against the past, current, and future passcodes
                if (!string.IsNullOrWhiteSpace(otp))
                {
                    if (TimeSensitivePassCode.GetOtps(code).Any(t => t.Equals(otp)))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}