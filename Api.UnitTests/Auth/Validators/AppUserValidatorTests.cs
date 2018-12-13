using Api.Auth;
using Api.Auth.Validators;
using Moq;
using NUnit.Framework;
using System.Data.Entity;
using System.Linq;

namespace Api.UnitTests.Auth.Validators
{
    [TestFixture(Category = TestHelper.Constants.TestCategories.UnitTests)]
    [Parallelizable(ParallelScope.Children)]
    public class AppUserValidatorTests
    {
        private AppUserValidator _appUserValidator;

        #region Setup

        [OneTimeSetUp]
        public void SetupAppUserValidator()
        {
            var dbContext = new Mock<DbContext>("Some garbage connection string").Object;
            var appUserStore = new Mock<ApplicationUserStore>(dbContext).Object;
            var appUserManger = new Mock<AppUserManager>(appUserStore).Object;
            _appUserValidator = new AppUserValidator(appUserManger);
        }


        [OneTimeTearDown]
        public void TearDown()
        {
            _appUserValidator = null;
        }

        #endregion

        #region AppUserValidator Constructor tests

        [Test]
        public void AppUserValidator_AllowsOnlyAlphanumericUserNames()
        {
            Assert.IsTrue(_appUserValidator.AllowOnlyAlphanumericUserNames);
        }


        [Test]
        public void AppUserValidator_RequireUniqueEmail()
        {
            Assert.IsTrue(_appUserValidator.RequireUniqueEmail);
        }

        #endregion

        #region AllowedEmailDomains tests

        [Test]
        public void AppUserValidator_AllowedEmailDomains_MustBeInLowerCase()
        {
            foreach (var domain in _appUserValidator.AllowedEmailDomains)
            {
                Assert.AreEqual(domain.ToLowerInvariant(), domain);
            }
        }

        [Test]
        public void AppUserValidator_AllowedEmailDomains_MustContain_Outlook()
        {
            Assert.IsTrue(_appUserValidator.AllowedEmailDomains.Contains("outlook.com"));
        }

        [Test]
        public void AppUserValidator_AllowedEmailDomains_MustContain_Live()
        {
            Assert.IsTrue(_appUserValidator.AllowedEmailDomains.Contains("live.com"));
        }

        [Test]
        public void AppUserValidator_AllowedEmailDomains_MustContain_Hotmail()
        {
            Assert.IsTrue(_appUserValidator.AllowedEmailDomains.Contains("hotmail.com"));
        }

        [Test]
        public void AppUserValidator_AllowedEmailDomains_MustContain_Gmail()
        {
            Assert.IsTrue(_appUserValidator.AllowedEmailDomains.Contains("gmail.com"));
        }

        [Test]
        public void AppUserValidator_AllowedEmailDomains_MustContain_Google()
        {
            Assert.IsTrue(_appUserValidator.AllowedEmailDomains.Contains("google.com"));
        }

        [Test]
        public void AppUserValidator_AllowedEmailDomains_MustContain_Yahoo()
        {
            Assert.IsTrue(_appUserValidator.AllowedEmailDomains.Contains("yahoo.com"));
        }

        [Test]
        public void AppUserValidator_AllowedEmailDomains_MustContain_Ymail()
        {
            Assert.IsTrue(_appUserValidator.AllowedEmailDomains.Contains("ymail.com"));
        }

        [Test]
        public void AppUserValidator_AllowedEmailDomains_MustContain_Microsoft()
        {
            Assert.IsTrue(_appUserValidator.AllowedEmailDomains.Contains("microsoft.com"));
        }

        [Test]
        public void AppUserValidator_AllowedEmailDomains_MustContain_YahooCoIn()
        {
            Assert.IsTrue(_appUserValidator.AllowedEmailDomains.Contains("yahoo.co.in"));
        }

        #endregion

    }
}
