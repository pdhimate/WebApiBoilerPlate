using Api.Data.Models;
using Api.Services.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Api.UnitTests.Services.Logging
{
    [TestFixture(Category = TestHelper.Constants.TestCategories.UnitTests)]
    [Parallelizable(ParallelScope.Children)]
    public class LogEntriesCreatorTests
    {
        #region Constants

        private const string HttpContextKey = "MS_HttpContext";
        private const string HttpOwinContextKey = "MS_OwinContext";

        #endregion


        #region LogEntriesCreator constructor tests

        [Test]
        public void LogEntriesCreator_HasValidHttpContextKey()
        {
            Assert.AreEqual(HttpContextKey, LogEntriesCreator.HttpContextKey);
        }

        [Test]
        public void LogEntriesCreator_HasValidHttpOwinContextKey()
        {
            Assert.AreEqual(HttpOwinContextKey, LogEntriesCreator.HttpOwinContextKey);
        }

        #endregion

        #region CreateHttpRequestEntry tests

        [Test]
        public void CreateHttpRequestEntry_ForNullHttpRequest_ReturnsNull()
        {
            var logEntriesCreator = new LogEntriesCreator();
            var httpRequestMessageEntry = logEntriesCreator.CreateHttpRequestEntry(null);
            Assert.IsNull(httpRequestMessageEntry);
        }

        [Test]
        public void CreateHttpRequestEntry_ForValidHttpRequest_ReturnsHttpRequestEntry()
        {
            var logEntriesCreator = new LogEntriesCreator();
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, $"https://SomeGarbage.uri/{Guid.NewGuid()}");
            var httpRequestMessageEntry = logEntriesCreator.CreateHttpRequestEntry(httpRequestMessage);

            Assert.IsNotNull(httpRequestMessageEntry);
        }

        [Test]
        public void CreateHttpRequestEntry_ForHttpRequestWithValidOAuthHeader_ReturnsHttpRequestEntryWithObfuscatedOAuthHeader()
        {
            var logEntriesCreator = new LogEntriesCreator();
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, $"https://SomeGarbage.uri/{Guid.NewGuid()}");
            httpRequestMessage.Headers.Add("Authorization", "Bearer C4Dnth4qootjv7p9LlN2_gHQ-TbqwFT516RWe3vStwAbWCpVgQCkduHcSwAPKu9ZhAUc0on6ZmX6iToBke1U1M53NjyYyF5Hv1aqDtk7zw8T0VxY1EUbGi9AwLyI-AylLijf53cKAxr-I5QoYORY1ScGplIw22_c6K-i4mMseAFzfucyMy9vWcOwPn7BIuHXvZPwhncLTMbazwoHwBgOiNEgg_t_NSNFMqA-DppUm3kim8Bmvprpsml0mTFNb9re7u--7Jf8JrynvXY7CacC5qoq8W8FGvqbYRtdv9vV5bOOVbBcI7cuYCT7M-OaN38DKpuq4Qzd63O-DatclNknH5SnKcklKlzEARfwL4HmQJjPfnt2ugNl24wIaeUpoE9QqDocsyyq05kR_jN1yKg_ZnzgipA9D7w_2qi6gpO0gJB2aXYy4QgivoxfTRFxzMsx3uaF2f8e56OnCu1GDyDw");
            var httpRequestMessageEntry = logEntriesCreator.CreateHttpRequestEntry(httpRequestMessage);

            Assert.AreEqual("Authorization: Bearer xxxxxxxxx", httpRequestMessageEntry.Headers);
        }

        [Test]
        public void CreateHttpRequestEntry_ForHttpRequestWithOAuthHeaderWithEmptyAcccessToken_ReturnsHttpRequestEntryWithEmptyOAuthHeader()
        {
            var logEntriesCreator = new LogEntriesCreator();
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, $"https://SomeGarbage.uri/{Guid.NewGuid()}");
            httpRequestMessage.Headers.Add("Authorization", "Bearer ");
            var httpRequestMessageEntry = logEntriesCreator.CreateHttpRequestEntry(httpRequestMessage);

            Assert.AreEqual("Authorization: Bearer", httpRequestMessageEntry.Headers);
        }

        [Test]
        public void CreateHttpRequestEntry_ForHttpRequestWithIpAddress_ExtractsTheIpAddress()
        {
            var ipAddress = "11.22.33.44";
            var logEntriesCreator = new LogEntriesCreator();
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, $"https://SomeGarbage.uri/{Guid.NewGuid()}");
            dynamic httpContextWrapper = new ExpandoObject();
            httpContextWrapper.Request = new ExpandoObject();
            httpContextWrapper.Request.RemoteIpAddress = ipAddress;

            httpRequestMessage.Properties.Add(HttpOwinContextKey, httpContextWrapper);
            var httpRequestMessageEntry = logEntriesCreator.CreateHttpRequestEntry(httpRequestMessage);

            Assert.AreEqual(ipAddress, httpRequestMessageEntry.IpAddress);
        }

        [Test]
        public void CreateHttpRequestEntry_ForHttpRequestWithIdentity_ExtractsuserName()
        {
            var userName = "someUserName";
            var logEntriesCreator = new LogEntriesCreator();
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, $"https://SomeGarbage.uri/{Guid.NewGuid()}");
            var httpContextWrapper = new HttpContextWrapper(new HttpContext(new HttpRequest("", $"https://SomeGarbage.uri/{Guid.NewGuid()}", ""), new HttpResponse(null)));

            var userMock = new Mock<IPrincipal>();
            var identityMock = new Mock<IIdentity>();
            identityMock.SetupGet(u => u.Name).Returns(userName);
            userMock.SetupGet(u => u.Identity).Returns(identityMock.Object);
            userMock.SetupGet(u => u.Identity).Returns(identityMock.Object);
            httpContextWrapper.User = userMock.Object;

            httpRequestMessage.Properties.Add(HttpContextKey, httpContextWrapper);
            var httpRequestMessageEntry = logEntriesCreator.CreateHttpRequestEntry(httpRequestMessage);

            Assert.AreEqual(userName, httpRequestMessageEntry.UserName);
        }


        #endregion
    }
}
