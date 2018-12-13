using Api.BusinessService.Common;
using Api.Controllers;
using Moq;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace Api.UnitTests.Controllers
{
    [TestFixture(Category = TestHelper.Constants.TestCategories.UnitTests)]
    [Parallelizable(ParallelScope.Children)]
    public class AccountControllerTests
    {
        [Test]
        public void UserExitsAction_MustHave_AllowAnonymousAttribute()
        {
            var controller = new AccountController(new Mock<IAccountService>().Object);
            var hasAllowAnonymousAttribute = Helper.ActionHasAttribute<AccountController, IHttpActionResult, AllowAnonymousAttribute>(
                (x) => controller.UserExists(null));
            Assert.IsTrue(hasAllowAnonymousAttribute);
        }

        [Test]
        public void RegisterUserAction_MustHave_AllowAnonymousAttribute()
        {
            var controller = new AccountController(new Mock<IAccountService>().Object);
            var hasAllowAnonymousAttribute = Helper.ActionHasAttribute<AccountController, Task<IHttpActionResult>, AllowAnonymousAttribute>(
                (x) => controller.RegisterUser(null));
            Assert.IsTrue(hasAllowAnonymousAttribute);
        }

        [Test]
        public void ForgotPasswordAction_MustHave_AllowAnonymousAttribute()
        {
            var hasAllowAnonymousAttribute =
                Helper.MethodHasAttribute<AccountController, AllowAnonymousAttribute>(nameof(AccountController.ForgotPassword));
            Assert.IsTrue(hasAllowAnonymousAttribute);
        }

        [Test]
        public void ResetPasswordAction_MustHave_AllowAnonymousAttribute()
        {
            var controller = new AccountController(new Mock<IAccountService>().Object);
            var hasAllowAnonymousAttribute = Helper.ActionHasAttribute<AccountController, IHttpActionResult, AllowAnonymousAttribute>(
                (x) => controller.ResetPassword(null));
            Assert.IsTrue(hasAllowAnonymousAttribute);
        }
    }
}
