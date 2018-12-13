using Api.Auth;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.UnitTests.Auth
{
    [TestFixture(Category = TestHelper.Constants.TestCategories.UnitTests)]
    [Parallelizable(ParallelScope.Children)]
    public class TimeSensitivePassCodeTests
    {
        private const string _permittedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";

        #region GenerateSharedPrivateKey tests

        [Test]
        public void GenerateSharedPrivateKey_ReturnNonEmptyKey()
        {
            var key = TimeSensitivePassCode.GenerateSharedPrivateKey();
            Assert.IsFalse(string.IsNullOrWhiteSpace(key));
        }

        [Test]
        public void GenerateSharedPrivateKey_ReturnsA16CharKey()
        {
            var key = TimeSensitivePassCode.GenerateSharedPrivateKey();
            Assert.IsTrue(key.Length == 16);
        }

        [Test]
        public void GenerateSharedPrivateKey_ReturnsKeyWithPermittedChars()
        {
            var key = TimeSensitivePassCode.GenerateSharedPrivateKey();
            Assert.IsTrue(key.All(c => _permittedChars.Contains(c)));
        }

        #endregion

        #region GetOtps tests

        [Test]
        public void GetOtps_Returns3Otps()
        {
            var key = TimeSensitivePassCode.GenerateSharedPrivateKey();
            var otps = TimeSensitivePassCode.GetOtps(key);

            Assert.AreEqual(3, otps.Count);
        }

        [Test]
        public void GetOtps_ReturnsOtpsWithLength6()
        {
            var key = TimeSensitivePassCode.GenerateSharedPrivateKey();
            var otps = TimeSensitivePassCode.GetOtps(key);

            foreach (var otp in otps)
            {
                Assert.AreEqual(6, otp.Length);
            }
        }

        #endregion

    }
}
