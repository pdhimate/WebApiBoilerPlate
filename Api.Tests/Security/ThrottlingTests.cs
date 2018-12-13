using Api.App_Start;
using Api.BusinessEntities.AccountController;
using Api.BusinessEntities.Common;
using Api.IntegrationTests.Controllers;
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
    /// <summary>
    /// Note: The name starts with Z so that these tests are executed after all other tests have been executed.
    /// </summary>
    [TestFixture(Category = TestHelper.Constants.TestCategories.ApiIntegrationTests)]
    public class Z_ThrottlingTests : BaseHttpTest
    {
        #region Throttling test

        /// <summary>
        /// Note: The name starts with Z so that this test is executed after all other tests have been executed.
        /// </summary>
        [Test]
        public void Z1_UserExists_For_3RequestsIn1Second_Returns_TooManyRequests()
        {
            // Wait for some time to avoid throlling error due to any previous tests
            DelayBy(3);

            var restClient = new RestClient(ApiBaseUrl);
            var objectToPost = new UserExistsReq { EmailId = TestHelper.Constants.DatabaseStaticEntities.AdminUserEmail };

            var requests = new List<RestRequest>();
            for (int i = 0; i < 3; i++)
            {
                RestRequest request = CreateRequest(AccountControllerTests.ServiceUrl,
                                                    AccountControllerTests.UserExitsActionUrl,
                                                    objectToPost);
                requests.Add(request);
            }

            var responsesLock = new object();
            var responses = new List<IRestResponse>();
            var tasks = requests.Select(req => new TaskFactory().StartNew(() =>
            {
                IRestResponse response = restClient.Post(req);
                lock (responsesLock)
                {
                    responses.Add(response);
                }
            }));
            var start = DateTime.UtcNow;
            Task.WaitAll(tasks.ToArray());
            var end = DateTime.UtcNow;
            var seconds = (end - start).TotalSeconds;

            Assert.IsTrue(seconds < 1); // Calls must have been made within 1 second only
            var tooManyRequestsResponse = responses.FirstOrDefault(r => (int)r.StatusCode == 429); // Too many requests
            Assert.IsNotNull(tooManyRequestsResponse);

            var apiResponse = JsonConvert.DeserializeObject<ApiResponseDto<object>>(tooManyRequestsResponse.Content);
            Assert.IsNotNull(apiResponse.Error); // must have error 
            Assert.IsNotNull(apiResponse.Error.Title, "Too many requests");
            Assert.IsNotNull(apiResponse.Error.Messages.Any(m => m.Contains("per Second")));
            Assert.IsNull(apiResponse.Result); // must not have result

        }


        /// <summary>
        /// Note: The name starts with Z so that this test is executed after all other tests have been executed.
        /// </summary>
        [Test]
        public void Z2_UserExists_For_31RequestsIn1Minute_ReturnsBadRequest()
        {
            // Wait for some time to avoid throlling error due to any previous tests
            DelayBy(3);

            var restClient = new RestClient(ApiBaseUrl);
            var objectToPost = new UserExistsReq { EmailId = TestHelper.Constants.DatabaseStaticEntities.AdminUserEmail };

            var requests = new List<RestRequest>();
            for (int i = 0; i < 31; i++)
            {
                RestRequest request = CreateRequest(AccountControllerTests.ServiceUrl,
                    AccountControllerTests.UserExitsActionUrl, objectToPost);
                requests.Add(request);
            }

            var responses = new List<IRestResponse>();

            var start = DateTime.UtcNow;
            foreach (var req in requests)
            {
                IRestResponse response = PostDelayed(restClient, req, 1);
                responses.Add(response);
            }
            var end = DateTime.UtcNow;
            var minutes = (end - start).TotalMinutes;

            Assert.IsTrue(minutes < 1); // Calls must have been made within 1 minute only
            var tooManyRequestsResponse = responses.FirstOrDefault(r => (int)r.StatusCode == 429); // Too many requests
            Assert.IsNotNull(tooManyRequestsResponse);

            var apiResponse = JsonConvert.DeserializeObject<ApiResponseDto<object>>(tooManyRequestsResponse.Content);
            Assert.IsNotNull(apiResponse.Error); // must have error 
            Assert.IsNotNull(apiResponse.Error.Title, "Too many requests");
            Assert.IsNotNull(apiResponse.Error.Messages.Any(m => m.Contains("per Minute")));
            Assert.IsNull(apiResponse.Result); // must not have result
        }

        #endregion

    }
}
