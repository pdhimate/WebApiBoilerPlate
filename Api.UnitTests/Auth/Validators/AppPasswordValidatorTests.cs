using Api.Auth.Validators;
using NUnit.Framework;

namespace Api.UnitTests.Auth.Validators
{
    [TestFixture(Category = TestHelper.Constants.TestCategories.UnitTests)]
    [Parallelizable(ParallelScope.Children)]
    public class AppPasswordValidatorTests
    {
        #region AppPasswordValidator Constructor tests

        [Test]
        public void AppPasswordValidator_RequiresAtLeast6CharPassword()
        {
            var passwordValidator = new AppPasswordValidator();
            Assert.AreEqual(6, passwordValidator.RequiredLength);
        }

        [Test]
        public void AppPasswordValidator_RequiresNonLetterOrDigit()
        {
            var passwordValidator = new AppPasswordValidator();
            Assert.IsTrue(passwordValidator.RequireNonLetterOrDigit);
        }

        [Test]
        public void AppPasswordValidator_DoesNot_RequireDigit()
        {
            var passwordValidator = new AppPasswordValidator();
            Assert.IsFalse(passwordValidator.RequireDigit);
        }

        [Test]
        public void AppPasswordValidator_RequiresLowercaseChar()
        {
            var passwordValidator = new AppPasswordValidator();
            Assert.IsTrue(passwordValidator.RequireLowercase);
        }

        [Test]
        public void AppPasswordValidator_RequiresUppercaseChar()
        {
            var passwordValidator = new AppPasswordValidator();
            Assert.IsTrue(passwordValidator.RequireUppercase);
        }

        #endregion
    }
}
