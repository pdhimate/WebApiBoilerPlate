using Api.BusinessEntities;
using Api.BusinessEntities.UserController;
using Api.BusinessService.Admin;
using Api.Data.Access;
using Api.Data.Access.Repositories;
using Api.Data.Access.Repositories.Security;
using Api.Data.Models.Security;
using Api.TestHelper;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Api.BusinessService.Tests.Admin
{
    [TestFixture(Category = Constants.TestCategories.UnitTests)]

    public class UserServiceTests
    {
        #region Fields

        private List<AppUser> _appUsers;

        private const int MaxUsersCount = 15;

        #endregion

        #region Test setup

        [OneTimeSetUp]
        public void Setup()
        {
            _appUsers = DummyDataGenerator.GetUsers(MaxUsersCount, null);
        }

        private IUserService SetupUserService(Mock<IUserRepository> appUserRepository)
        {
            var unitOfWork = new Mock<IUnitOfWork>();
            unitOfWork.SetupGet(u => u.AppUsers).Returns(appUserRepository.Object);
            unitOfWork.Setup(u => u.Save()).Returns(It.IsAny<int>());
            return new UserService(unitOfWork.Object);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _appUsers = null;
        }

        #endregion

        #region GetUserById Tests

        [Test]
        public void GetUserById_ForAnExistingUserId_ReturnsTheUser()
        {
            // Init
            var appUserRepository = new Mock<IUserRepository>();
            appUserRepository.Setup(repo => repo.GetById(It.IsAny<long>()))
                             .Returns(new Func<long, AppUser>(id => _appUsers.Find(p => p.Id.Equals(id))));
            var userService = SetupUserService(appUserRepository);

            // Execute
            var userId = MaxUsersCount;
            var userFromService = userService.GetById(userId);

            // Assert
            Assert.IsNotNull(userFromService);
            var userInDb = _appUsers.First(u => u.Id == userId);
            Assert.AreEqual(userFromService.Email, userInDb.Email);
        }

        [Test]
        public void GetUserById_ForNonExistingUserId_Throws()
        {
            // Init
            var appUserRepository = new Mock<IUserRepository>();
            appUserRepository.Setup(repo => repo.GetById(It.IsAny<long>()))
                             .Returns(default(AppUser));
            var userService = SetupUserService(appUserRepository);

            // Execute
            var userId = MaxUsersCount + MaxUsersCount;
            Assert.Throws<BusinnessException>(() => userService.GetById(userId));
        }


        [Test]
        public void GetUserById_UserIdEqualToZero_Throws()
        {
            // Init
            var appUserRepository = new Mock<IUserRepository>();
            appUserRepository.Setup(repo => repo.GetById(It.IsAny<long>()))
                             .Returns(default(AppUser));
            var userService = SetupUserService(appUserRepository);

            // Execute
            var userId = 0;
            Assert.Throws<ArgumentOutOfRangeException>(() => userService.GetById(userId));
        }

        [Test]
        public void GetUserById_UserIdLessThanZero_Throws()
        {
            // Init
            var appUserRepository = new Mock<IUserRepository>();
            appUserRepository.Setup(repo => repo.GetById(It.IsAny<long>()))
                             .Returns(default(AppUser));
            var userService = SetupUserService(appUserRepository);

            // Execute
            var userId = -1;
            Assert.Throws<ArgumentOutOfRangeException>(() => userService.GetById(userId));
        }

        #endregion GetUserById Tests

        #region GetUserByEmail tests

        [Test]
        public void GetUserByEmail_ForAnExistingUserEmail_ReturnsTheUser()
        {
            // Init
            var appUserRepository = new Mock<IUserRepository>();
            appUserRepository.Setup(repo => repo.GetByEmail(It.IsAny<string>()))
                             .Returns(new Func<string, AppUser>(email => _appUsers.Find(p => p.Email.Equals(email))));
            var userService = SetupUserService(appUserRepository);

            // Execute
            var emailId = _appUsers[0].Email;
            var user = userService.GetUserByEmail(emailId);

            // Assert
            Assert.IsNotNull(user);
            Assert.AreEqual(user.Email, emailId);
        }

        [Test]
        public void GetUserByEmail_ForNonExistingUserEmail_Throws()
        {
            // Init
            var appUserRepository = new Mock<IUserRepository>();
            appUserRepository.Setup(repo => repo.GetByEmail(It.IsAny<string>()))
                             .Returns(default(AppUser));
            var userService = SetupUserService(appUserRepository);

            // Execute & Assert
            var emailId = Guid.NewGuid().ToString() + _appUsers[0].Email;
            Assert.Throws<BusinnessException>(() => userService.GetUserByEmail(emailId));
        }

        #endregion GetUserByEmail tests

        #region GetUsers tests

        [Test]
        public void GetUsers_ForPageNumberAndPageSize_GreaterThanZero_ReturnsPaginatedUsers()
        {
            // Init
            var pageNumber = 1;
            int pageSize = 2;
            var appUserRepository = new Mock<IUserRepository>();
            appUserRepository.Setup(repo => repo.GetByPage(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Expression<Func<AppUser, long>>>()))
                             .Returns(new Page<AppUser>(_appUsers.Take(pageSize), _appUsers.Count(), pageNumber, pageSize));
            var userService = SetupUserService(appUserRepository);

            // Execute
            var paginatedResult = userService.GetUsers(pageNumber, pageSize);

            // Assert
            Assert.IsNotNull(paginatedResult);
            Assert.IsNotNull(paginatedResult.Items);
            Assert.IsTrue(paginatedResult.Items.Count() > 0);
        }

        [Test]
        public void GetUsers_ForPageNumberAndPageSize_LessThanZero_Throws()
        {
            // Init
            var appUserRepository = new Mock<IUserRepository>();
            appUserRepository.Setup(repo => repo.GetUsers(It.IsAny<int>(), It.IsAny<int>()))
                             .Returns(default(Page<AppUser>));
            var userService = SetupUserService(appUserRepository);

            // Execute
            var pageNumber = -1;
            int pageSize = -2;
            Assert.Throws<ArgumentOutOfRangeException>(() => userService.GetUsers(pageNumber, pageSize));
        }

        [Test]
        public void GetUsers_ForPageNumberAndPageSize_EqualToZero_Throws()
        {
            // Init
            var appUserRepository = new Mock<IUserRepository>();
            appUserRepository.Setup(repo => repo.GetUsers(It.IsAny<int>(), It.IsAny<int>()))
                             .Returns(default(Page<AppUser>));
            var userService = SetupUserService(appUserRepository);

            // Execute
            var pageNumber = 0;
            int pageSize = 0;
            Assert.Throws<ArgumentOutOfRangeException>(() => userService.GetUsers(pageNumber, pageSize));
        }

        [Test]
        public void GetUsers_ForPageNumber_EqualToZero_Throws()
        {
            // Init
            var appUserRepository = new Mock<IUserRepository>();
            appUserRepository.Setup(repo => repo.GetUsers(It.IsAny<int>(), It.IsAny<int>()))
                             .Returns(default(Page<AppUser>));
            var userService = SetupUserService(appUserRepository);

            // Execute
            var pageNumber = 0;
            int pageSize = 2;
            Assert.Throws<ArgumentOutOfRangeException>(() => userService.GetUsers(pageNumber, pageSize));
        }

        [Test]
        public void GetUsers_ForPageSize_EqualToZero_Throws()
        {
            // Init
            var appUserRepository = new Mock<IUserRepository>();
            appUserRepository.Setup(repo => repo.GetUsers(It.IsAny<int>(), It.IsAny<int>()))
                             .Returns(default(Page<AppUser>));
            var userService = SetupUserService(appUserRepository);

            // Execute
            var pageNumber = 2;
            int pageSize = 0;
            Assert.Throws<ArgumentOutOfRangeException>(() => userService.GetUsers(pageNumber, pageSize));
        }

        [Test]
        public void GetUsers_ForPageNumber_LessThanZero_Throws()
        {
            // Init
            var appUserRepository = new Mock<IUserRepository>();
            appUserRepository.Setup(repo => repo.GetUsers(It.IsAny<int>(), It.IsAny<int>()))
                             .Returns(default(Page<AppUser>));
            var userService = SetupUserService(appUserRepository);

            // Execute
            var pageNumber = -1;
            int pageSize = 2;
            Assert.Throws<ArgumentOutOfRangeException>(() => userService.GetUsers(pageNumber, pageSize));
        }

        [Test]
        public void GetUsers_ForPageSize_LessThanZero_Throws()
        {
            // Init
            var appUserRepository = new Mock<IUserRepository>();
            appUserRepository.Setup(repo => repo.GetUsers(It.IsAny<int>(), It.IsAny<int>()))
                             .Returns(default(Page<AppUser>));
            var userService = SetupUserService(appUserRepository);

            // Execute
            var pageNumber = -1;
            int pageSize = 0;
            Assert.Throws<ArgumentOutOfRangeException>(() => userService.GetUsers(pageNumber, pageSize));
        }

        #endregion GetUsers tests

        #region UpdateUser tests

        [Test]
        public void UpdateUser_ForAnExistingUserIdAndUser_ReturnsTrue()
        {
            // Init
            var appUserRepository = new Mock<IUserRepository>();
            appUserRepository.Setup(repo => repo.GetById(It.IsAny<long>()))
                             .Returns(new Func<long, AppUser>(id => _appUsers.Find(p => p.Id.Equals(id))));
            appUserRepository.Setup(repo => repo.Update(It.IsAny<AppUser>()));
            var userService = SetupUserService(appUserRepository);

            // Execute
            var userId = MaxUsersCount;
            var userToUpdatedto = new UserResDto
            {
                Email = "updateTestEmail@a.a",
                FirstName = "update first name",
                LastName = "update last name",
                MiddleName = "update middle name",
                PhoneNumber = "update phone number",
                AddressId = 1
            };
            var result = userService.UpdateUser(userId, userToUpdatedto);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void UpdateUser_ForNonExistingUserId_Throws()
        {
            // Init
            var appUserRepository = new Mock<IUserRepository>();
            appUserRepository.Setup(repo => repo.GetById(It.IsAny<long>()))
                             .Returns(new Func<long, AppUser>(id => _appUsers.Find(p => p.Id.Equals(id))));
            appUserRepository.Setup(repo => repo.Update(It.IsAny<AppUser>()));
            var userService = SetupUserService(appUserRepository);

            // Execute
            var userId = MaxUsersCount + MaxUsersCount;
            var userToUpdatedto = new UserResDto
            {
                Email = "updateTestEmail@a.a",
                FirstName = "update first name",
                LastName = "update last name",
                MiddleName = "update middle name",
                PhoneNumber = "update phone number",
                AddressId = 1
            };

            // Assert
            Assert.Throws<BusinnessException>(() => userService.UpdateUser(userId, userToUpdatedto));
        }

        [Test]
        public void UpdateUser_ForUserIdEqualtoZero_Throws()
        {
            // Init
            var appUserRepository = new Mock<IUserRepository>();
            appUserRepository.Setup(repo => repo.GetById(It.IsAny<long>()))
                             .Returns(new Func<long, AppUser>(id => _appUsers.Find(p => p.Id.Equals(id))));
            appUserRepository.Setup(repo => repo.Update(It.IsAny<AppUser>()));
            var userService = SetupUserService(appUserRepository);

            // Execute
            var userId = 0;
            var userToUpdatedto = new UserResDto
            {
                Email = "updateTestEmail@a.a",
                FirstName = "update first name",
                LastName = "update last name",
                MiddleName = "update middle name",
                PhoneNumber = "update phone number",
                AddressId = 1
            };

            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => userService.UpdateUser(userId, userToUpdatedto));
        }

        [Test]
        public void UpdateUser_ForUserIdLessThanZero_Throws()
        {
            // Init
            var appUserRepository = new Mock<IUserRepository>();
            appUserRepository.Setup(repo => repo.GetById(It.IsAny<long>()))
                             .Returns(new Func<long, AppUser>(id => _appUsers.Find(p => p.Id.Equals(id))));
            appUserRepository.Setup(repo => repo.Update(It.IsAny<AppUser>()));
            var userService = SetupUserService(appUserRepository);

            // Execute
            var userId = -1;
            var userToUpdatedto = new UserResDto
            {
                Email = "updateTestEmail@a.a",
                FirstName = "update first name",
                LastName = "update last name",
                MiddleName = "update middle name",
                PhoneNumber = "update phone number",
                AddressId = 1
            };

            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => userService.UpdateUser(userId, userToUpdatedto));
        }

        [Test]
        public void UpdateUser_ForExistingUserId_AndNullUserDto_Throws()
        {
            // Init
            var appUserRepository = new Mock<IUserRepository>();
            appUserRepository.Setup(repo => repo.GetById(It.IsAny<long>()))
                             .Returns(new Func<long, AppUser>(id => _appUsers.Find(p => p.Id.Equals(id))));
            appUserRepository.Setup(repo => repo.Update(It.IsAny<AppUser>()));
            var userService = SetupUserService(appUserRepository);

            // Execute
            var userId = MaxUsersCount;
            UserResDto userToUpdatedto = null;

            // Assert
            Assert.Throws<ArgumentNullException>(() => userService.UpdateUser(userId, userToUpdatedto));
        }


        #endregion UpdateUser tests
    }
}
