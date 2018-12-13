using Api.Filters;
using Api.GlobalHandlers;
using Microsoft.Owin.Security.OAuth;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Filters;
using WebApiThrottle;

namespace Api.UnitTests.App_Start
{
    [TestFixture(Category = TestHelper.Constants.TestCategories.UnitTests)]
    [Parallelizable(ParallelScope.Children)]
    public class WebApiConfigTests
    {
        [Test]
        public void Register_Adds_ValidateModelAttribute_ToAllApiActions()
        {
            var config = new HttpConfiguration();
            WebApiConfig.Register(config);
            var hasValidateModelAttribute = config.Filters.Any(f => f.Instance is ValidateModelAttribute);
            Assert.IsTrue(hasValidateModelAttribute);
        }

        [Test]
        public void Register_Adds_AuthorizeAttribute_ToAllApiActions()
        {
            var config = new HttpConfiguration();
            WebApiConfig.Register(config);
            var hasValidateModelAttribute = config.Filters.Any(f => f.Instance is AuthorizeAttribute);
            Assert.IsTrue(hasValidateModelAttribute);
        }

        [Test]
        public void Register_Adds_HostAuthenticationFilter_ToAllApiActions()
        {
            var config = new HttpConfiguration();
            WebApiConfig.Register(config);
            var hasHostAuthenticationFilter = config.Filters.Any(f => f.Instance is HostAuthenticationFilter);
            Assert.IsTrue(hasHostAuthenticationFilter);
        }

        [Test]
        public void Register_Adds_HostAuthenticationFilter_WithBearerType_ToAllApiActions()
        {
            var config = new HttpConfiguration();
            WebApiConfig.Register(config);
            var hostAuthenticationFilter = config.Filters.First(f => f.Instance is HostAuthenticationFilter).Instance;

            Assert.AreEqual(OAuthDefaults.AuthenticationType, (hostAuthenticationFilter as HostAuthenticationFilter).AuthenticationType);
        }

        [Test]
        public void Register_Adds_RequireHttpsHandler_ForAllApiActions()
        {
            var config = new HttpConfiguration();
            WebApiConfig.Register(config);
            var hasRequireHttpsHandler = config.MessageHandlers.Any(f => f is RequireHttpsHandler);
            Assert.IsTrue(hasRequireHttpsHandler);
        }

        [Test]
        public void Register_Adds_ApiResponseWrapper_ForAllApiActions()
        {
            var config = new HttpConfiguration();
            WebApiConfig.Register(config);
            var hasApiResponseWrapper = config.MessageHandlers.Any(f => f is ApiResponseWrapper);
            Assert.IsTrue(hasApiResponseWrapper);
        }

        [Test]
        public void Register_Adds_Throttling_ForAllApiActions()
        {
            var config = new HttpConfiguration();
            WebApiConfig.Register(config);
            var throttlingHandler = config.MessageHandlers.FirstOrDefault(f => f is ThrottlingHandler);
            Assert.IsNotNull(throttlingHandler);
        }

    }
}
