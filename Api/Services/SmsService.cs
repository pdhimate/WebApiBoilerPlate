using Api.BusinessService;
using Microsoft.AspNet.Identity;
using System;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Api.Services
{
    /// <summary>
    /// Provides for sending SMS
    /// </summary>
    public class TwilioSmsService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            TwilioClient.Init(Constants.Twilio.AccountSid, Constants.Twilio.AuthToken);
            ModifyDestinationforTrialAccount(message);

            // Send a new outgoing SMS by POSTing to the Messages resource
            var result = await MessageResource.CreateAsync(
                             from: new PhoneNumber(Constants.Twilio.PhoneNumber), // From number, must be an SMS-enabled Twilio number
                             to: new PhoneNumber(message.Destination), // To number, if using Sandbox see note above
                             body: message.Body);   // Message content

            HandleException(result);
        }

        /// <summary>
        /// When using trial account SMS can only be sent to the verified numbers.
        /// </summary>
        /// <param name="message"></param>
        private static void ModifyDestinationforTrialAccount(IdentityMessage message)
        {
#if DEBUG
            message.Destination = Constants.Twilio.TrialAccountPhoneNumber;
#endif
        }

        #region Private helpers

        private static void HandleException(MessageResource messageResult)
        {
            // Status is : Queued, Sending, Sent, Failed or null if the number is not valid
            if (messageResult.ErrorCode.HasValue)
            {
                var twilioException = new Exception($"Twilio error code: {messageResult.ErrorCode.Value}. Twilio message: {messageResult.ErrorMessage}. Message sent on date: {messageResult.DateSent}.");
                throw new BusinnessException($"Could not send the sms to {messageResult.To}.", twilioException);
            }
        }

        #endregion
    }
}