using Api.BusinessService.Admin;
using Api.Controllers;
using Api.Controllers.Admin;
using Api.Data.Models.Security;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Api.UnitTests.Controllers.Admin
{
    [TestFixture(Category = TestHelper.Constants.TestCategories.UnitTests)]
    [Parallelizable(ParallelScope.Children)]
    public class RefreshTokenControllerTests
    {
        [Test]
        public void RefreshTokenController_MustHave_AuthorizeAttribute()
        {
            var controller = new RefreshTokenController(new Mock<IRefreshTokenService>().Object);
            var hasAuthorizeAttribute = Helper.ControllerHasAttribute<RefreshTokenController, AuthorizeAttribute>(controller);
            Assert.IsTrue(hasAuthorizeAttribute);
        }

        [Test]
        public void RefreshTokenController_MustHave_AuthorizeAttribute_WithAdminRole()
        {
            var controller = new RefreshTokenController(new Mock<IRefreshTokenService>().Object);
            var authorizeAttribute = Helper.GetAttribute<RefreshTokenController, AuthorizeAttribute>(controller);
            Assert.IsTrue(authorizeAttribute.Roles.Contains(AppRoles.AdminRole));
        }
    }
}
