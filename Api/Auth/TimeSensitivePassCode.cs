using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace Api.Auth
{
    /// <summary>
    /// Provides to genearation and validation of a Time based OTP (which is based on HMAC based OTP).
    /// For more details refer: http://bitoftech.net/2014/10/15/two-factor-authentication-asp-net-web-api-angularjs-google-authenticator/
    /// </summary>
    public static class TimeSensitivePassCode
    {
        /// <summary>
        /// Generated a Time based OTP, which would be shared with the user as a secret key.
        /// </summary>
        /// <returns>TOPT: shared secret</returns>
        public static string GenerateSharedPrivateKey()
        {
            byte[] key = new byte[10]; // 80 bits
            using (var rngProvider = new RNGCryptoServiceProvider())
            {
                rngProvider.GetBytes(key);
            }

            return key.ToBase32String();
        }

        /// <summary>
        /// Gets previous, current and next OTPs corresponding to the specified seceret.
        /// </summary>
        /// <param name="base32EncodedSecret"></param>
        /// <returns>A set of 2 OTPs. Previous, current and next.</returns>
        public static IList<string> GetOtps(string base32EncodedSecret)
        {
            DateTime epochStart = new DateTime(1970, 01, 01, 0, 0, 0, 0, DateTimeKind.Utc);
            long counter = (long)Math.Floor((DateTime.UtcNow - epochStart).TotalSeconds / 30);
            var otps = new List<string>();

            otps.Add(GetHotp(base32EncodedSecret, counter - 1)); // previous OTP
            otps.Add(GetHotp(base32EncodedSecret, counter)); // current OTP
            otps.Add(GetHotp(base32EncodedSecret, counter + 1)); // next OTP

            return otps;
        }

        /// <summary>
        /// Generates a HMAC based OTP
        /// </summary>
        /// <param name="base32EncodedSecret"></param>
        /// <param name="counter"></param>
        /// <returns>An HOTP</returns>
        private static string GetHotp(string base32EncodedSecret, long counter)
        {
            byte[] message = BitConverter.GetBytes(counter).Reverse().ToArray(); //Intel machine (little endian) 
            byte[] secret = base32EncodedSecret.ToByteArray();

            var hmac = new HMACSHA1(secret, true);

            byte[] hash = hmac.ComputeHash(message);
            int offset = hash[hash.Length - 1] & 0xf;
            int truncatedHash = ((hash[offset] & 0x7f) << 24) |
            ((hash[offset + 1] & 0xff) << 16) |
            ((hash[offset + 2] & 0xff) << 8) |
            (hash[offset + 3] & 0xff);

            int hotp = truncatedHash % 1000000;
            return hotp.ToString().PadLeft(6, '0');
        }
    }

    /// <summary>
    /// Provides for base32 based encoding of bytes.
    /// We choose base32 since Google Autheticator uses the same.
    /// </summary>
    public static class StringHelper
    {
        /// <summary>
        /// The generated key would consist of these characters only.
        /// The numbers which might confuse the users with letters, have been omitted 
        /// </summary>
        private static string _supportedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";

        /// <summary>
        /// Encodes the specified bytes with base32 encoding.
        /// </summary>
        /// <param name="secret"></param>
        /// <returns></returns>
        public static string ToBase32String(this byte[] secret)
        {
            var bits = secret.Select(b => Convert.ToString(b, 2).PadLeft(8, '0'))
                             .Aggregate((a, b) => a + b);

            return Enumerable.Range(0, bits.Length / 5)
                             .Select(i => _supportedChars.Substring(Convert.ToInt32(bits.Substring(i * 5, 5), 2), 1))
                             .Aggregate((a, b) => a + b);
        }

        /// <summary>
        /// Decodes the specified bsae32-encoded string.
        /// </summary>
        /// <param name="secret"></param>
        /// <returns></returns>
        public static byte[] ToByteArray(this string secret)
        {
            var bits = secret.ToUpper().ToCharArray().Select(c => Convert.ToString(_supportedChars.IndexOf(c), 2).PadLeft(5, '0')).Aggregate((a, b) => a + b);

            return Enumerable.Range(0, bits.Length / 8).Select(i => Convert.ToByte(bits.Substring(i * 8, 8), 2)).ToArray();
        }

    }
}