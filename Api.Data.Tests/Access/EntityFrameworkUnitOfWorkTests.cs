 using Api.Data.Access;
using NUnit.Framework;
using System.Linq;

namespace Api.Data.IntegrationTests.Access
{
    [TestFixture(Category = TestHelper.Constants.TestCategories.DatabaseIntegrationTests)]
    public class EntityFrameworkUnitOfWorkTests : BaseTest
    {
        [Test]
        public void AdminUserMustExists_InDatabase()
        {
            using (var uow = new EntityFrameworkUnitOfWork())
            {
                var users = uow.AppUsers.GetUsers(1, 1);

                // There should be an admin user in the database
                Assert.IsNotNull(users);
                Assert.IsNotNull(users.Items.FirstOrDefault(u => u.UserName == Data.Constants.DefaultUsers.AdminUserName));
                Assert.IsTrue(users.Items.Count() == 1);
            }
        }

    }
}
