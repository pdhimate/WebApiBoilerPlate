using Api.Controllers;
using Microsoft.AspNet.Identity;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Results;

namespace Api.UnitTests.Controllers
{
    [TestFixture(Category = TestHelper.Constants.TestCategories.UnitTests)]
    [Parallelizable(ParallelScope.Children)]
    public class BaseApiControllerTests
    {
        [Test]
        public void BadRequestWithIdentityErrors_WithNullIdentityResult_Throws()
        {
            var baseApiController = new Mock<BaseApiController>().Object;
            Assert.Throws<ArgumentNullException>(() => baseApiController.BadRequestWithIdentityErrors(null));
        }

        [Test]
        public void BadRequestWithIdentityErrors_WithIdentityResultSucceeded_Throws()
        {
            var baseApiController = new Mock<BaseApiController>().Object;
            var identityResultMock = new Mock<IdentityResult>(true);
            Assert.Throws<InvalidOperationException>(() => baseApiController.BadRequestWithIdentityErrors(identityResultMock.Object));
        }

        [Test]
        public void BadRequestWithIdentityErrors_WithIdentityResultErrors_DoesNotThrow()
        {
            var baseApiController = new Mock<BaseApiController>();
            var errors = new List<string>
            {
                "Some error message 1",
                "Some error message 2"
            };
            var identityResultMock = new Mock<IdentityResult>(errors);
            Assert.DoesNotThrow(() => baseApiController.Object.BadRequestWithIdentityErrors(identityResultMock.Object));
        }

        [Test]
        public void BadRequestWithIdentityErrors_MustHave_ApiExplorerSettingsAttribute()
        {
            var baseApiController = new Mock<BaseApiController>().Object;
            var hasApiExplorerSettingsAttribute = Helper.ActionHasAttribute<BaseApiController, IHttpActionResult, ApiExplorerSettingsAttribute>(
                (x) => baseApiController.BadRequestWithIdentityErrors(null));
            Assert.IsTrue(hasApiExplorerSettingsAttribute);
        }

        [Test]
        public void BadRequestWithIdentityErrors_MustHave_ApiExplorerSettingsAttribute_WithIgnoreApiSetToTrue()
        {
            var baseApiController = new Mock<BaseApiController>().Object;
            var apiExplorerSettingsAttribute = Helper.GetAttribute<BaseApiController, IHttpActionResult, ApiExplorerSettingsAttribute>(
                    (x) => baseApiController.BadRequestWithIdentityErrors(null));
            Assert.IsTrue(apiExplorerSettingsAttribute.IgnoreApi);
        }

        [Test]
        public void BaseApiController_MustHave_ApiExplorerSettingsAttribute_WithIgnoreApiSetToTrue_ForAllPublicActions()
        {
            var baseApiController = new Mock<BaseApiController>().Object;
            var publicActions = Helper.GetPublicActions<IHttpActionResult>(baseApiController);

            foreach (var action in publicActions)
            {
                var apiExplorerSettingsAttribute = Helper.GetAttribute<ApiExplorerSettingsAttribute>(action);
                Assert.IsTrue(apiExplorerSettingsAttribute.IgnoreApi);
            }
        }
    }
}
