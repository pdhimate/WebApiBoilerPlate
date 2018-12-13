using Api.Resources;
using Microsoft.AspNet.Identity;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Configuration;
using System.Net;
using System.Threading.Tasks;

namespace Api.Services
{
    /// <summary>
    /// Provides for sending emails.
    /// </summary>
    public class SendgridEmailService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            SendGridMessage sgMessage = CreateMail(message);
            var sendGridClient = new SendGridClient(Constants.SendGrid.ApiKey);
            Response response = await sendGridClient.SendEmailAsync(sgMessage);

            // TODO: uncomment this to handle. do this when you have correct send grid keys and valid sender address.
            //if (response == null || response.StatusCode != HttpStatusCode.Accepted)
            //{
            //    throw new ApplicationException($"Failed to send an email to the user: {message.Destination}. Result: {response}");
            //}
        }

        private static SendGridMessage CreateMail(IdentityMessage message)
        {
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(Emails.DefaultSenderAddress, "Api Team"), // TODO: change name of sender
                Subject = message.Subject,
                PlainTextContent = message.Body,
                //HtmlContent = "<strong>Hello, Email!</strong>"
            };
            msg.AddTo(new EmailAddress(message.Destination, null)); // TODO: Add name of receipient

            return msg;
        }
    }
}