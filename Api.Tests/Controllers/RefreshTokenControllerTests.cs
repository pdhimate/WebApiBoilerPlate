using Api.BusinessEntities.Common;
using Api.BusinessEntities.RefreshTokenController;
using Api.Data.Models.Security;
using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.IntegrationTests.Controllers
{
    [TestFixture(Category = TestHelper.Constants.TestCategories.ApiIntegrationTests)]
    public class RefreshTokenControllerTests : BaseHttpTest
    {
        internal static readonly string ServiceUrl = "/api/RefreshToken";

        #region Tokens and delete

        public static readonly string TokensActionUrl = "/tokens";

        public static readonly string DeleteActionUrl = "/delete";

        [Test]
        public void DeleteRefreshToken_ById_Passes()
        {
            // Generate a token, so that a token gets saved in the db
            var adminUserName = TestHelper.Constants.DatabaseStaticEntities.AdminUserName;
            var password = TestHelper.Constants.DatabaseStaticEntities.DefaultPassword;
            var accesstoken = GetBearerAccessToken(adminUserName, password, ClientId_WebApp);

            // Get tokens
            ApiResponseDto<PagedRes<RefreshTokenRes>> apiResponse = GetRefreshTokens(accesstoken);

            // Must have token for admin user
            var adminsToken = apiResponse.Result.Items.First(t => t.UserName == adminUserName);
            Assert.IsNotNull(adminsToken);

            // Delete token
            var restClient = new RestClient(ApiBaseUrl);
            var reqObj = new DeleteRefreshTokenReq { Id = adminsToken.Id };
            var request = CreateRequest(ServiceUrl, DeleteActionUrl, reqObj, accesstoken);
            var response = restClient.Delete(request);
            var jsonContent = response.Content;
            var deleteResponse = JsonConvert.DeserializeObject<ApiResponseDto<bool>>(jsonContent);
            Assert.IsTrue(deleteResponse.Result);

            // Get tokens
            apiResponse = GetRefreshTokens(accesstoken);
            // Must not have token for admin user
            if (apiResponse.Result != null)
            {
                adminsToken = apiResponse.Result.Items.First(t => t.UserName == adminUserName);
                Assert.IsNull(adminsToken);
            }
            else
            {
                Assert.IsNull(apiResponse.Result);
            }
        }

        #endregion


        private static ApiResponseDto<PagedRes<RefreshTokenRes>> GetRefreshTokens(string accesstoken)
        {
            var restClient = new RestClient(ApiBaseUrl);
            var pageReq = new PagedReqDto();
            pageReq.PageNumber = 1;
            pageReq.PageSize = 10;
            RestRequest request = CreateRequest(ServiceUrl, TokensActionUrl, pageReq, accesstoken);

            IRestResponse response = restClient.Post(request);
            string jsonContent = response.Content;
            var apiResponse = JsonConvert.DeserializeObject<ApiResponseDto<PagedRes<RefreshTokenRes>>>(jsonContent);
            return apiResponse;
        }

    }
}
