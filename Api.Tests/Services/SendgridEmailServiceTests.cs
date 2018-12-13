using Api.Services;
using Microsoft.AspNet.Identity;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Api.IntegrationTests.Services
{
    [TestFixture(Category = TestHelper.Constants.TestCategories.ApiIntegrationTests)]
    public class SendgridEmailServiceTests
    {
        /// <summary>
        /// Note: Only add ids which belong to you, here. 
        /// Otherwise other ids would be getting spam emails. 
        /// This could block our account.
        /// </summary>
        private List<string> validEmailIds = new List<string>
        {
            "YourEmailId@SomeEmailProvider.com",
        };

        [Test]
        [Ignore("Do not run this test, unless the email provider has changed. Let us not waste the free emails capacity.")]
        public void SendAsync_ToValidEmailId_DoesNotThrow()
        {
            IIdentityMessageService sendGridMessageService = new SendgridEmailService();
            var identityMessage = new IdentityMessage();
            identityMessage.Subject = "Integration test for sending email.";
            identityMessage.Body = "Just testing. Delete this email.";
            identityMessage.Destination = validEmailIds.First();

            Assert.DoesNotThrowAsync(() => sendGridMessageService.SendAsync(identityMessage));
        }

        [Test]
        [Ignore("Do not run this test, unless the email provider has changed. The failed emails lead to blocking of the account.")]
        public void SendAsync_ToInvalidEmailId_Throws()
        {
            IIdentityMessageService sendGridMessageService = new SendgridEmailService();
            var identityMessage = new IdentityMessage();
            identityMessage.Subject = "Integration test for sending email.";
            identityMessage.Body = "Just testing. Delete this email.";
            identityMessage.Destination = "some invalid email id";

            Assert.ThrowsAsync<ApplicationException>(() => sendGridMessageService.SendAsync(identityMessage));
        }

        [Test]
        [Ignore("Do not run this test, unless the email provider has changed. The failed emails lead to blocking of the account.")]
        public void SendAsync_ToEmptyEmailId_Throws()
        {
            IIdentityMessageService sendGridMessageService = new SendgridEmailService();
            var identityMessage = new IdentityMessage();
            identityMessage.Subject = "Integration test for sending email.";
            identityMessage.Body = "Just testing. Delete this email.";
            identityMessage.Destination = string.Empty;

            Assert.ThrowsAsync<ApplicationException>(() => sendGridMessageService.SendAsync(identityMessage));
        }
    }
}
