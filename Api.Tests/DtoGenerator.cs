using Api.BusinessEntities;
using Api.TestHelper;
using Api.IntegrationTests.Controllers;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Api.BusinessEntities.AccountController;

namespace Api.IntegrationTests
{
    /// <summary>
    /// Generates test data using the <see cref="BusinessEntities"/>.
    /// </summary>
    internal class DtoGenerator
    {
        #region Local constants

        internal const string TestStringPrefix = "Test_";
        internal const string TestPhonePrefix = "12345";
        internal const string TestUserPassword = "abcd12#$!";
        internal const string TestEmailDomain = "gmail.com";

        private static readonly Random _random = new Random(DateTime.UtcNow.Millisecond);

        #endregion

        #region Singleton, Threadsafe Id counter

        private static readonly object IdLock = new object();
        private static long _id = 1;
        private static long Id
        {
            get
            {
                return _id;
            }
            set
            {
                lock (IdLock)
                {
                    _id = value;
                }
            }
        }

        #endregion

        /// <summary>
        /// Creates a new RegisterUserDto with unique ids.
        /// </summary>
        /// <returns></returns>
        public static RegisterUserReq GetValidRegisterUserDto()
        {
            var userName = string.Empty;
            var email = string.Empty;
            GetUnique(ref userName, ref email);

            var registerUserDto = new RegisterUserReq
            {
                Email = email,
                UserName = userName,
                Password = TestUserPassword,
                TwoFactorEnabled = false,
            };

            return registerUserDto;
        }

        #region Private helper methods

        private static void GetUnique(ref string userName, ref string email)
        {
            var userExits = true;
            while (userExits)
            {
                userName = TestStringPrefix + ++Id;
                email = $"{userName}@{ TestEmailDomain}";
                userExits = BaseHttpTest.CheckIfUserExists(email);
            }
        }

        private static string GetRandomPhoneNumber()
        {
            var phone = TestPhonePrefix + _random.Next(12345, 99999);
            return phone;
        }

        #endregion
    }
}
