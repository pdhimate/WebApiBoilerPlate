using Api.App_Start;
using Api.Filters;
using Api.GlobalHandlers;
using Microsoft.Owin.Security.OAuth;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Filters;
using WebApiThrottle;

namespace Api.UnitTests.App_Start
{
    [TestFixture(Category = TestHelper.Constants.TestCategories.UnitTests)]
    [Parallelizable(ParallelScope.Children)]
    public class ThrottlingConfigTests
    {
        #region Calls Per unit time tests
        [Test]
        public void CallsPerSecond_Is_Set_To_2()
        {
            Assert.AreEqual(ThrottlingConfig.CallsPerSecond, 2);
        }

        [Test]
        public void CallsPerMinute_Is_Set_To_30()
        {
            Assert.AreEqual(ThrottlingConfig.CallsPerMinute, 30);
        }

        [Test]
        public void CallsPerHour_Is_Set_To_1200()
        {
            Assert.AreEqual(ThrottlingConfig.CallsPerHour, 1200);
        }

        [Test]
        public void CallsPerDay_Is_Set_To_16000()
        {
            Assert.AreEqual(ThrottlingConfig.CallsPerDay, 16000);
        }

        [Test]
        public void CallsPerWeek_Is_Set_To_Null()
        {
            Assert.AreEqual(ThrottlingConfig.CallsPerWeek, null);
        }

        #endregion

        #region GetDefaultPolicy tests

        [Test]
        public void GetDefaultPolicy_ReturnsPolicy_Throttling_WithIpThrottle()
        {
            var defaultPolicy = ThrottlingConfig.GetDefaultPolicy();
            Assert.IsTrue(defaultPolicy.IpThrottling);
        }

        [Test]
        public void GetDefaultPolicy_ReturnsPolicy_Throttling_WithEndpointThrottle()
        {
            var defaultPolicy = ThrottlingConfig.GetDefaultPolicy();
            Assert.IsTrue(defaultPolicy.EndpointThrottling);
        }

        #endregion

    }
}
