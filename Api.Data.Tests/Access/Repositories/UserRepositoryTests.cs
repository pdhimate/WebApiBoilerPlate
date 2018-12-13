using Api.Data.Access.Repositories;
using Api.Data.Access.Repositories.Security;
using Api.Data.Models;
using Api.Data.Models.Security;
using Api.TestHelper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Data.IntegrationTests.Access.Repositories
{
    [TestFixture(Category = TestHelper.Constants.TestCategories.DatabaseIntegrationTests)]
    public class UserRepositoryTests : BaseTest
    {
        /// <summary>
        /// The number of users that have been added to the database for testing.
        /// Note: The number has been used to write the various tests below.
        ///       Changing this number would mean that the tests need to be revised.
        /// </summary>
        private const int UsersCount = 20;

        #region Test methods Setup

        public override void SetUp()
        {
            base.SetUp();
            AddTestUsers();
        }

        private void AddTestUsers()
        {
            var userStore = new UserStore<AppUser, AppRole, long, ExternalUserLogin, AppUserAppRoleMapping, AppUserClaim>(new AppDatabaseContext());
            var userManager = new UserManager<AppUser, long>(userStore);
            var dbContext = new AppDatabaseContext();
            var addresses = dbContext.Addresses.ToList();
            var testUsers = DummyDataGenerator.GetUsers(UsersCount, addresses);

            foreach (var user in testUsers)
            {
                userManager.Create(user, Constants.DefaultUsers.DefaultTestUserPassword);
            }
        }

        #endregion

        #region GetUsers 

        [Test]
        public void GetUsers_ForPageNo0PageSize0_Throws()
        {
            var userRepository = new UserRepository(new AppDatabaseContext());
            var pageNumber = 0;
            var pageSize = 0;
            Assert.Throws<ArgumentOutOfRangeException>(() => userRepository.GetUsers(pageNumber, pageSize));
        }

        [Test]
        public void GetUsers_ForPageNo0_Throws()
        {
            var userRepository = new UserRepository(new AppDatabaseContext());
            var pageNumber = 0;
            var pageSize = 1;
            Assert.Throws<ArgumentOutOfRangeException>(() => userRepository.GetUsers(pageNumber, pageSize));
        }

        [Test]
        public void GetUsers_ForPageSize0_Throws()
        {
            var userRepository = new UserRepository(new AppDatabaseContext());
            var pageNumber = 1;
            var pageSize = 0;
            Assert.Throws<ArgumentOutOfRangeException>(() => userRepository.GetUsers(pageNumber, pageSize));
        }

        [Test]
        public void GetUsers_ForNegativePageNo_Throws()
        {
            var userRepository = new UserRepository(new AppDatabaseContext());
            var pageNumber = -1;
            var pageSize = 1;
            Assert.Throws<ArgumentOutOfRangeException>(() => userRepository.GetUsers(pageNumber, pageSize));
        }

        [Test]
        public void GetUsers_ForNegativePageSize_Throws()
        {
            var userRepository = new UserRepository(new AppDatabaseContext());
            var pageNumber = 1;
            var pageSize = -1;
            Assert.Throws<ArgumentOutOfRangeException>(() => userRepository.GetUsers(pageNumber, pageSize));
        }

        [Test]
        public void GetUsers_ForPageNo1PageSize1_Returns1User()
        {
            var userRepository = new UserRepository(new AppDatabaseContext());
            var pageNumber = 1;
            var pageSize = 1;
            var pagedResult = userRepository.GetUsers(pageNumber, pageSize);

            Assert.IsNotNull(pagedResult);
            Assert.IsNotNull(pagedResult.Items);
            Assert.AreEqual(pageSize, pagedResult.Items.Count());
            Assert.AreEqual(pageNumber, pagedResult.PageNumber);
            Assert.AreEqual(pageSize, pagedResult.PageSize);
            Assert.AreEqual(UsersCount + 1, pagedResult.TotalCount); // +1 for the admin user
        }

        [Test]
        public void GetUsers_ForPageNo2PageSize5_Returns5Users()
        {
            var userRepository = new UserRepository(new AppDatabaseContext());
            var pageNumber = 2;
            var pageSize = 5;
            var pagedResult = userRepository.GetUsers(pageNumber, pageSize);

            Assert.IsNotNull(pagedResult);
            Assert.IsNotNull(pagedResult.Items);
            Assert.AreEqual(pageSize, pagedResult.Items.Count());
            Assert.AreEqual(pageNumber, pagedResult.PageNumber);
            Assert.AreEqual(pageSize, pagedResult.PageSize);
            Assert.AreEqual(UsersCount + 1, pagedResult.TotalCount); // +1 for the admin user
        }

        [Test]
        public void GetUsers_ForPageNo50000PageSize2_Returns0Users()
        {
            var userRepository = new UserRepository(new AppDatabaseContext());
            var pageNumber = 50000;
            var pageSize = 2;
            var pagedResult = userRepository.GetUsers(pageNumber, pageSize);

            Assert.IsNotNull(pagedResult);
            Assert.IsNotNull(pagedResult.Items);
            Assert.AreEqual(0, pagedResult.Items.Count());
            Assert.AreEqual(pageNumber, pagedResult.PageNumber);
            Assert.AreEqual(pageSize, pagedResult.PageSize);
            Assert.AreEqual(UsersCount + 1, pagedResult.TotalCount); // +1 for the admin user
        }


        [Test]
        public void GetUsers_ForPageNo3PageSize200_Returns0Users()
        {
            var userRepository = new UserRepository(new AppDatabaseContext());
            var pageNumber = 3;
            var pageSize = 200;
            var pagedResult = userRepository.GetUsers(pageNumber, pageSize);

            Assert.IsNotNull(pagedResult);
            Assert.IsNotNull(pagedResult.Items);
            Assert.AreEqual(0, pagedResult.Items.Count());
            Assert.AreEqual(pageNumber, pagedResult.PageNumber);
            Assert.AreEqual(pageSize, pagedResult.PageSize);
            Assert.AreEqual(UsersCount + 1, pagedResult.TotalCount); // +1 for the admin user
        }

        [Test]
        public void GetUsers_ForPageNo1PageSize200_ReturnsAllTheUsers()
        {
            var userRepository = new UserRepository(new AppDatabaseContext());
            var pageNumber = 1;
            var pageSize = 200;
            var pagedResult = userRepository.GetUsers(pageNumber, pageSize);

            Assert.IsNotNull(pagedResult);
            Assert.IsNotNull(pagedResult.Items);
            Assert.AreEqual(UsersCount + 1, pagedResult.Items.Count());
            Assert.AreEqual(pageNumber, pagedResult.PageNumber);
            Assert.AreEqual(pageSize, pagedResult.PageSize);
            Assert.AreEqual(UsersCount + 1, pagedResult.TotalCount); // +1 for the admin user
        }
        #endregion
    }
}
