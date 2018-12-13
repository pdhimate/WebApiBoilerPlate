using Api.App_Start;
using Api.BusinessEntities.AccountController;
using Api.BusinessEntities.Common;
using Api.TestHelper;
using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;


namespace Api.IntegrationTests.Security
{
    [TestFixture(Category = TestHelper.Constants.TestCategories.ApiIntegrationTests)]
    public class TokenApiTests : BaseHttpTest
    {
        internal static readonly string ServiceUrl = "/api/token";

        #region Client Id tests

        [Test]
        public void RegisteredUser_WithEmailConfirmed_WithoutClientId_CannotGenerateBearerToken()
        {
            var adminUserName = TestHelper.Constants.DatabaseStaticEntities.AdminUserName;
            var password = TestHelper.Constants.DatabaseStaticEntities.DefaultPassword;
            Assert.Throws<Exception>(() => GetBearerAccessToken(adminUserName, password, string.Empty));
        }

        [Test]
        public void RegisteredUser_WithEmailConfirmed_WithInactiveClientId_CannotGenerateBearerToken()
        {
            var adminUserName = TestHelper.Constants.DatabaseStaticEntities.AdminUserName;
            var password = TestHelper.Constants.DatabaseStaticEntities.DefaultPassword;
            Assert.Throws<Exception>(() => GetBearerAccessToken(adminUserName, password, ClientId_WebApp_Inactive));
        }

        [Test]
        public void RegisteredUser_WithEmailConfirmed_WithInvalidClientId_CannotGenerateBearerToken()
        {
            var adminUserName = TestHelper.Constants.DatabaseStaticEntities.AdminUserName;
            var password = TestHelper.Constants.DatabaseStaticEntities.DefaultPassword;
            Assert.Throws<Exception>(() => GetBearerAccessToken(adminUserName, password, "blah blah!"));
        }

        [Test]
        public void RegisteredUser_WithEmalConfirmed_WithValidClientId_CannotGenerateBearerToken()
        {
            var adminUserName = TestHelper.Constants.DatabaseStaticEntities.AdminUserName;
            var password = TestHelper.Constants.DatabaseStaticEntities.DefaultPassword;
            var accessToken = GetBearerAccessToken(adminUserName, password, ClientId_WebApp);

            Assert.IsFalse(string.IsNullOrWhiteSpace(accessToken));
        }

        #endregion

        #region Access token tests

        [Test]
        public void NewlyRegisteredUser_WithEmailNotConfirmed_WithValidClientId_CannotGenerateBearerToken()
        {
            var newUser = RegisterNewUser();
            Assert.Throws<Exception>(() => GetBearerAccessToken(newUser.UserName, newUser.Password, ClientId_WebApp));
        }

        [Test]
        public void RegisteredUser_WithEmailConfirmed_WithValidClientId_CanGenerateBearerToken()
        {
            var adminUserName = TestHelper.Constants.DatabaseStaticEntities.AdminUserName;
            var password = TestHelper.Constants.DatabaseStaticEntities.DefaultPassword;
            var token = GetBearerAccessToken(adminUserName, password, ClientId_WebApp);

            Assert.IsFalse(string.IsNullOrWhiteSpace(token));
        }

        #endregion

        #region Refresh token tests

        [Test]
        public void NewlyRegisteredUser_WithEmailNotConfirmed_WithValidClientId_CannotGenerateRefreshToken()
        {
            var newUser = RegisterNewUser();
            Assert.Throws<Exception>(() => GetRefreshToken(newUser.UserName, newUser.Password, ClientId_WebApp));
        }

        [Test]
        public void RegisteredUser_WithEmailConfirmed_WithValidClientId_CanGenerateRefreshToken()
        {
            var adminUserName = TestHelper.Constants.DatabaseStaticEntities.AdminUserName;
            var password = TestHelper.Constants.DatabaseStaticEntities.DefaultPassword;
            var token = GetRefreshToken(adminUserName, password, ClientId_WebApp);

            Assert.IsFalse(string.IsNullOrWhiteSpace(token));
        }


        [Test]
        public void RegisteredUser_WithEmailConfirmed_WithValidClientId_CanGenerateAcceesToken_UsingRefreshToken()
        {
            var adminUserName = TestHelper.Constants.DatabaseStaticEntities.AdminUserName;
            var password = TestHelper.Constants.DatabaseStaticEntities.DefaultPassword;
            var refreshToken = GetRefreshToken(adminUserName, password, ClientId_WebApp);

            Assert.IsFalse(string.IsNullOrWhiteSpace(refreshToken));
        }

        #endregion
    }
}
