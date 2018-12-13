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

namespace Api.UnitTests.Controllers
{
    [TestFixture(Category = TestHelper.Constants.TestCategories.UnitTests)]
    [Parallelizable(ParallelScope.Children)]
    public class UserControllerTests
    {
        [Test]
        public void UserController_MustHave_AuthorizeAttribute()
        {
            var controller = new UserController(new Mock<IUserService>().Object);
            var hasAuthorizeAttribute = Helper.ControllerHasAttribute<UserController, AuthorizeAttribute>(controller);
            Assert.IsTrue(hasAuthorizeAttribute);
        }

        [Test]
        public void UserController_MustHave_AuthorizeAttribute_WithAdminRole()
        {
            var controller = new UserController(new Mock<IUserService>().Object);
            var authorizeAttribute = Helper.GetAttribute<UserController, AuthorizeAttribute>(controller);
            Assert.IsTrue(authorizeAttribute.Roles.Contains(AppRoles.AdminRole));
        }
    }
}
