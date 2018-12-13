using System.Linq;
using NUnit.Framework;
using Api.Data.Models.Security;

namespace Api.Data.IntegrationTests.Models.Security
{
    [TestFixture(Category = TestHelper.Constants.TestCategories.DatabaseIntegrationTests)]
    [Parallelizable(ParallelScope.Children)]
    public class AppRoleTests
    {
        #region GetAll

        [Test]
        public void GetAll_Returns_1DistinctRole()
        {
            var roles = AppRoles.GetAll();

            Assert.AreEqual(1, roles.Distinct().Count());
        }

        #endregion

        #region Roles strings

        [Test]
        public void AppRoles_Has_AdminRole()
        {
            Assert.AreEqual("Admin", AppRoles.AdminRole);
        }

        #endregion
    }
}
