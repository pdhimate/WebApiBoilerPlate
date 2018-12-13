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

namespace Api.IntegrationTests.Controllers
{
    [TestFixture(Category = TestHelper.Constants.TestCategories.ApiIntegrationTests)]
    public class AccountControllerTests : BaseHttpTest
    {
        internal static readonly string ServiceUrl = "/api/Account";

        #region UserExists tests

        internal static readonly string UserExitsActionUrl = "/UserExists";

        [Test]
        public void UserExists_ForAdminUser_OverHttp_ReturnsHttpForbidden()
        {
            var httpApiUrl = "http" + ApiBaseUrl.TrimStart("https".ToCharArray());
            var restClient = new RestClient(httpApiUrl);
            var objectToPost = new UserExistsReq { EmailId = TestHelper.Constants.DatabaseStaticEntities.AdminUserEmail };
            RestRequest request = CreateRequestDelayed(ServiceUrl, UserExitsActionUrl, objectToPost);

            IRestResponse response = restClient.Post(request);
            string jsonContent = response.Content;
            var apiResponse = JsonConvert.DeserializeObject<ApiResponseDto<object>>(jsonContent);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.Forbidden);
            Assert.IsNotNull(apiResponse.Error);
            Assert.IsNull(apiResponse.Result);
        }

        [Test]
        public void UserExists_ForAdminUser_ReturnsTrue()
        {
            var restClient = new RestClient(ApiBaseUrl);
            var objectToPost = new UserExistsReq { EmailId = TestHelper.Constants.DatabaseStaticEntities.AdminUserEmail };
            RestRequest request = CreateRequestDelayed(ServiceUrl, UserExitsActionUrl, objectToPost);

            IRestResponse response = restClient.Post(request);
            string jsonContent = response.Content;
            var apiResponse = JsonConvert.DeserializeObject<ApiResponseDto<bool>>(jsonContent);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            Assert.IsNull(apiResponse.Error);
            Assert.IsTrue(apiResponse.Result == true);
        }

        [Test]
        public void UserExists_ForNonExistingUser_ReturnsFalse()
        {
            var restClient = new RestClient(ApiBaseUrl);
            var objectToPost = new UserExistsReq { EmailId = $"abc@{Guid.NewGuid().ToString()}.com" };
            RestRequest request = CreateRequestDelayed(ServiceUrl, UserExitsActionUrl, objectToPost);


            IRestResponse response = restClient.Post(request);
            string jsonContent = response.Content;
            var apiResponse = JsonConvert.DeserializeObject<ApiResponseDto<bool>>(jsonContent);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            Assert.IsNull(apiResponse.Error);
            Assert.IsTrue(apiResponse.Result == false);
        }

        [Test]
        public void UserExists_ForNullEmailId_ReturnsBadRequest()
        {
            var restClient = new RestClient(ApiBaseUrl);
            var objectToPost = new UserExistsReq { EmailId = null };
            RestRequest request = CreateRequestDelayed(ServiceUrl, UserExitsActionUrl, objectToPost);

            IRestResponse response = restClient.Post(request);
            string content = response.Content;

            Assert.IsTrue(response.StatusCode == HttpStatusCode.BadRequest);
        }

        [Test]
        public void UserExists_ForEmptyEmailId_ReturnsBadRequest()
        {
            var restClient = new RestClient(ApiBaseUrl);
            var objectToPost = new UserExistsReq { EmailId = string.Empty };
            RestRequest request = CreateRequestDelayed(ServiceUrl, UserExitsActionUrl, objectToPost);

            IRestResponse response = restClient.Post(request);
            string content = response.Content;

            Assert.IsTrue(response.StatusCode == HttpStatusCode.BadRequest);
        }

        [Test]
        public void UserExists_ForInvalidEmailId_ReturnsBadRequest()
        {
            var restClient = new RestClient(ApiBaseUrl);
            var objectToPost = new UserExistsReq { EmailId = Guid.NewGuid().ToString() };
            RestRequest request = CreateRequestDelayed(ServiceUrl, UserExitsActionUrl, objectToPost);

            IRestResponse response = restClient.Post(request);
            string content = response.Content;

            Assert.IsTrue(response.StatusCode == HttpStatusCode.BadRequest);
        }

        #endregion UserExists tests

        #region RegisterUser tests

        public static readonly string RegisterActionUrl = "/RegisterUser";

        [Test]
        public void RegisterUser_WithValidEmailAndPassword_ReturnsOk()
        {
            var restClient = new RestClient(ApiBaseUrl);
            var newUser = DtoGenerator.GetValidRegisterUserDto();
            RestRequest request = CreateRequest(ServiceUrl, RegisterActionUrl, newUser);

            IRestResponse response = restClient.Post(request);
            string jsonContent = response.Content;
            var apiResponse = JsonConvert.DeserializeObject<ApiResponseDto<RegisterUserRes>>(jsonContent);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsNull(apiResponse.Error);
            Assert.IsNotNull(apiResponse.Result.Message);
        }

        [Test]
        public void RegisterUser_WithInvalidEmail_ReturnsBadRequest()
        {
            var restClient = new RestClient(ApiBaseUrl);
            var newUser = DtoGenerator.GetValidRegisterUserDto();
            newUser.Email = Guid.NewGuid().ToString();
            RestRequest request = CreateRequest(ServiceUrl, RegisterActionUrl, newUser);

            IRestResponse response = restClient.Post(request);
            string jsonContent = response.Content;
            var apiResponse = JsonConvert.DeserializeObject<ApiResponseDto<RegisterUserRes>>(jsonContent);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.IsNull(apiResponse.Result);
            Assert.IsNotNull(apiResponse.Error);
            Assert.IsTrue(apiResponse.Error.Messages.Count > 0);
        }

        [Test]
        public void RegisterUser_WithNullPassword_ReturnsBadRequest()
        {
            var restClient = new RestClient(ApiBaseUrl);
            var newUser = DtoGenerator.GetValidRegisterUserDto();
            newUser.Password = null;
            RestRequest request = CreateRequest(ServiceUrl, RegisterActionUrl, newUser);

            IRestResponse response = restClient.Post(request);
            string jsonContent = response.Content;
            var apiResponse = JsonConvert.DeserializeObject<ApiResponseDto<RegisterUserRes>>(jsonContent);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.IsNull(apiResponse.Result);
            Assert.IsNotNull(apiResponse.Error);
            Assert.IsTrue(apiResponse.Error.Messages.Count > 0);
        }

        [Test]
        public void RegisterUser_WithEmptyPassword_ReturnsBadRequest()
        {
            var restClient = new RestClient(ApiBaseUrl);
            var newUser = DtoGenerator.GetValidRegisterUserDto();
            newUser.Password = string.Empty;
            RestRequest request = CreateRequest(ServiceUrl, RegisterActionUrl, newUser);

            IRestResponse response = restClient.Post(request);
            string jsonContent = response.Content;
            var apiResponse = JsonConvert.DeserializeObject<ApiResponseDto<RegisterUserRes>>(jsonContent);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.IsNull(apiResponse.Result);
            Assert.IsNotNull(apiResponse.Error);
            Assert.IsTrue(apiResponse.Error.Messages.Count > 0);
        }

        [Test]
        public void RegisterUser_WithAlphabeticalPassword_ReturnsBadRequest()
        {
            var restClient = new RestClient(ApiBaseUrl);
            var newUser = DtoGenerator.GetValidRegisterUserDto();
            newUser.Password = "password";
            RestRequest request = CreateRequest(ServiceUrl, RegisterActionUrl, newUser);

            IRestResponse response = restClient.Post(request);
            string jsonContent = response.Content;
            var apiResponse = JsonConvert.DeserializeObject<ApiResponseDto<RegisterUserRes>>(jsonContent);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.IsNull(apiResponse.Result);
            Assert.IsNotNull(apiResponse.Error);
            Assert.IsTrue(apiResponse.Error.Messages.Count > 0);
        }

        [Test]
        public void RegisterUser_WithNumericPassword_ReturnsBadRequest()
        {
            var restClient = new RestClient(ApiBaseUrl);
            var newUser = DtoGenerator.GetValidRegisterUserDto();
            newUser.Password = "123456";
            RestRequest request = CreateRequest(ServiceUrl, RegisterActionUrl, newUser);

            IRestResponse response = restClient.Post(request);
            string jsonContent = response.Content;
            var apiResponse = JsonConvert.DeserializeObject<ApiResponseDto<RegisterUserRes>>(jsonContent);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.IsNull(apiResponse.Result);
            Assert.IsNotNull(apiResponse.Error);
            Assert.IsTrue(apiResponse.Error.Messages.Count > 0);
        }

        #endregion RegisterUser tests

    }
}
