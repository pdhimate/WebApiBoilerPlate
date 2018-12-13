using Api.TestHelper;
using Api.IntegrationTests.Controllers;
using RestSharp;
using System;
using System.Configuration;
using System.Net;
using Api.BusinessEntities.AccountController;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using Api.BusinessEntities.Common;
using System.Collections.Concurrent;
using System.Threading;
using Api.App_Start;

namespace Api.IntegrationTests
{
    public class BaseHttpTest
    {
        protected static readonly string ApiBaseUrl = ConfigurationManager.AppSettings[TestHelper.Constants.Webconfig.ApiBaseUrlKey];
        protected static readonly string ClientId_WebApp = "AngularWebApp";
        protected static readonly string ClientId_WebApp_Inactive = "Tests-InactiveWebApp";

        public BaseHttpTest()
        {
            DisableSllCertificateValidation();
        }

        #region Public methods

        /// <summary>
        /// Checks if a user with the specified email id exists on the Api
        /// </summary>
        /// <param name="emailId"></param>
        /// <returns></returns>
        public static bool CheckIfUserExists(string emailId)
        {
            DelayBy((double)ThrottlingConfig.CallsPerSecond);

            var restClient = new RestClient(ApiBaseUrl);
            ConsoleOutput($"Created rest client [{restClient.BaseUrl}]");

            var objectToPost = new UserExistsReq { EmailId = emailId };
            RestRequest request = CreateRequest(AccountControllerTests.ServiceUrl, AccountControllerTests.UserExitsActionUrl, objectToPost);
            IRestResponse response = restClient.Post(request);
            ConsoleOutput($"Issued a [{request.Method}] to the resource [{request.Resource}]");

            string jsonContent = response.Content;
            ConsoleOutput($"Got response [{jsonContent}]");
            var apiResponse = JsonConvert.DeserializeObject<ApiResponseDto<bool>>(jsonContent);
            return apiResponse.Result;
        }

        #endregion

        #region Protected methods

        private static object _previousCallUtcLock = new object();
        private static DateTime? _previousCallUtc;

        /// <summary>
        /// Delays the execution by the specified seconds
        /// </summary>
        /// <param name="seconds"></param>
        protected static void DelayBy(double seconds)
        {
            lock (_previousCallUtcLock)
            {
                if (_previousCallUtc.HasValue)
                {
                    double secondsPassed = 0;
                    while (secondsPassed <= seconds)
                    {
                        int secondsToWait = (int)((seconds - secondsPassed) * 1000);
                        Thread.Sleep(secondsToWait);
                        secondsPassed = (DateTime.UtcNow - _previousCallUtc.Value).TotalSeconds;
                    }
                }
                else
                {
                    _previousCallUtc = DateTime.UtcNow;
                }
            }
        }

        /// <summary>
        /// Registers a new user on the Api and returns his credentials.
        /// </summary>
        /// <returns>The register user dto contaning credentials of the newly created user</returns>
        protected RegisterUserReq RegisterNewUser()
        {
            var restClient = new RestClient(ApiBaseUrl);
            var newUser = DtoGenerator.GetValidRegisterUserDto();
            RestRequest request = CreateRequest(AccountControllerTests.ServiceUrl, AccountControllerTests.RegisterActionUrl, newUser);

            IRestResponse response = restClient.Post(request);
            string jsonContent = response.Content;
            var apiResponse = JsonConvert.DeserializeObject<ApiResponseDto<RegisterUserRes>>(jsonContent);

            if (HttpStatusCode.OK != response.StatusCode)
            {
                throw new Exception($"Failed to register the user with email : {newUser.Email} and username : {newUser.UserName}");
            }

            return newUser;
        }

        /// <summary>
        /// Generates a bearer access token, which is used to autheticate the specified user credentials.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns>Access token</returns>
        protected string GetBearerAccessToken(string userName, string password, string clientId)
        {
            var getTokenObject = $"grant_type=password&username={userName}&password={password}&client_id={clientId}";
            Dictionary<string, object> responseDictionary = GenerateTokens(userName, getTokenObject);

            var accessToken = responseDictionary["access_token"] as string;
            if (string.IsNullOrWhiteSpace(accessToken))
            {
                throw new Exception($"Empty access token for the user : {userName}");
            }
            return accessToken;
        }

        protected string GetBearerAccessToken(string refreshToken, string clientId)
        {
            var getTokenObject = $"grant_type=refresh_token&refresh_token={refreshToken}&client_id={clientId}";
            Dictionary<string, object> responseDictionary = GenerateTokens(refreshToken, getTokenObject);

            var accessToken = responseDictionary["access_token"] as string;
            if (string.IsNullOrWhiteSpace(accessToken))
            {
                throw new Exception($"Empty access token for the refresh_token : {refreshToken}");
            }
            return accessToken;
        }

        /// <summary>
        /// Generates a refresh token, which is used to get new access tokens for the specified user credentials.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns>Access token</returns>
        protected string GetRefreshToken(string userName, string password, string clientId)
        {
            var getTokenObject = $"grant_type=password&username={userName}&password={password}&client_id={clientId}";
            Dictionary<string, object> responseDictionary = GenerateTokens(userName, getTokenObject);

            var refreshToken = responseDictionary["refresh_token"] as string;
            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                throw new Exception($"Empty refresh token for the user : {userName}");
            }
            return refreshToken;
        }

        private static Dictionary<string, object> GenerateTokens(string userName, string paramString)
        {
            var restClient = new RestClient(ApiBaseUrl);
            var request = new RestRequest("/api/token");
            request.AddParameter("application/x-www-form-urlencoded", paramString, ParameterType.RequestBody);

            IRestResponse response = restClient.Post(request);
            var responseDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(response.Content);
            if (responseDictionary.ContainsKey("error"))
            {
                var error = responseDictionary["error"] as string;
                if (!string.IsNullOrWhiteSpace(error))
                {
                    throw new Exception($"Error for the user: {userName}. Error : {error}");
                }
            }

            return responseDictionary;
        }

        #region Create request

        /// <summary>
        /// Creates a Http request without Authorizatrion header.
        /// </summary>
        /// <typeparam name="Tin"></typeparam>
        /// <param name="serviceUrl"></param>
        /// <param name="action"></param>
        /// <param name="objectToPost"></param>
        /// <returns></returns>
        protected static RestRequest CreateRequest<Tin>(string serviceUrl, string action, Tin objectToPost)
        {
            return CreateRequest(serviceUrl, action, objectToPost, null);
        }

        /// <summary>
        /// Creates a Http request without Authorizatrion header, after a certain delay.
        /// </summary>
        /// <typeparam name="Tin"></typeparam>
        /// <param name="serviceUrl"></param>
        /// <param name="action"></param>
        /// <param name="objectToPost"></param>
        /// <returns></returns>
        protected static RestRequest CreateRequestDelayed<Tin>(string serviceUrl, string action, Tin objectToPost)
        {
            DelayBy((double)ThrottlingConfig.CallsPerSecond);
            return CreateRequest(serviceUrl, action, objectToPost);
        }

        /// <summary>
        /// Creates a Http request with the specified access token in the Authorization header.
        /// Use this to create request that require logged in user.
        /// </summary>
        /// <typeparam name="Tin"></typeparam>
        /// <param name="serviceUrl"></param>
        /// <param name="action"></param>
        /// <param name="objectToPost"></param>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        protected static RestRequest CreateRequest<Tin>(string serviceUrl, string action, Tin objectToPost, string accessToken)
        {
            string actionEndpoint = serviceUrl + action;
            var request = new RestRequest(actionEndpoint);
            request.RequestFormat = DataFormat.Json;
            if (objectToPost != null)
            {
                request.AddBody(objectToPost, TestHelper.Constants.SerializationNamespaces.BusinessEntities);
            }
            if (!string.IsNullOrWhiteSpace(accessToken))
            {
                request.AddHeader(nameof(Authorization), $"Bearer {accessToken}");
            }
            return request;
        }

        /// <summary>
        /// Creates a Http request with the specified access token in the Authorization header, after a certain delay
        /// Use this to create request that require logged in user.
        /// </summary>
        /// <typeparam name="Tin"></typeparam>
        /// <param name="serviceUrl"></param>
        /// <param name="action"></param>
        /// <param name="objectToPost"></param>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        protected static RestRequest CreateRequestDelayed<Tin>(string serviceUrl, string action, Tin objectToPost, string accessToken)
        {
            DelayBy((double)ThrottlingConfig.CallsPerSecond);
            return CreateRequest(serviceUrl, action, objectToPost, accessToken);
        }

        #endregion

        #region Post

        public static IRestResponse PostDelayed(IRestClient client,
                                               IRestRequest request)
        {
            return PostDelayed(client, request, (double)ThrottlingConfig.CallsPerSecond);
        }

        public static IRestResponse PostDelayed(IRestClient client,
                                              IRestRequest request,
                                              double secondsToDelayBy)
        {
            DelayBy(secondsToDelayBy);
            return client.Post(request);
        }

        #endregion

        /// <summary>
        /// Displays the specified message on Console or Debug outputs
        /// </summary>
        /// <param name="message"></param>
        protected static void ConsoleOutput(string message)
        {
            Debug.WriteLine(message);
            Console.WriteLine(message);
            Trace.WriteLine(message);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// In debug mode, disable the SLL certificate validation.
        /// This allows to run IntegrationTests while deployed on local IIS.
        /// </summary>
        private void DisableSllCertificateValidation()
        {
#if DEBUG
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
#endif
        }

        #endregion


    }
}
