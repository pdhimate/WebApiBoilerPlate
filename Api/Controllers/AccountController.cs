using Api.Auth;
using Api.BusinessEntities;
using Api.BusinessEntities.AccountController;
using Api.BusinessEntities.Common;
using Api.BusinessService.Common;
using Api.Data.Models.Security;
using Api.Resources;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace Api.Controllers
{
    /// <summary>
    /// Provides API for registering new users and other account management related functions.
    /// </summary>
    public class AccountController : BaseApiController
    {
        #region Private dependencies

        private readonly IAccountService _accountService;

        #endregion

        #region Private Properties

        /// <summary>
        /// Allows for authentication for the current Request.
        /// </summary>
        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        #endregion

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        #region Api calls

        /// <summary>
        /// Checks if a user with the specified email id already exits
        /// </summary>
        /// <param name="userExistsDto"></param>
        /// <returns>bool</returns>
        [HttpPost]
        [AllowAnonymous]
        [ResponseType(typeof(ApiResponseDto<bool>))]
        public IHttpActionResult UserExists(UserExistsReq userExistsDto)
        {
            bool userAccountExits = _accountService.UserExists(userExistsDto);
            return Ok(userAccountExits);
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="registerUserDto"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [ResponseType(typeof(ApiResponseDto<BaseResponseDto>))]
        // TODO: Apply capcha
        public async Task<IHttpActionResult> RegisterUser(RegisterUserReq registerUserDto)
        {
            // Create user
            IdentityResult identityResult = AppUserManager.RegisterUser(registerUserDto);
            if (!identityResult.Succeeded)
            {
                return BadRequestWithIdentityErrors(identityResult);
            }

            // Send confirmation email
            AppUser appUser = await AppUserManager.FindByEmailAsync(registerUserDto.Email);
            var resultMessage = await SendEmailConfirmationAsync(appUser);

            // Configure reponse
            var response = new BaseResponseDto();
            response.Message = Responses.UserRegisteredMessage + " " + resultMessage;

            return Ok(response);
        }

        /// <summary>
        /// Sends a token to the email id of the logged in user. 
        /// This is used to confirm his email id.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ResponseType(typeof(ApiResponseDto<BaseResponseDto>))]
        // TODO: Apply custom throttling 
        public async Task<IHttpActionResult> SendConfirmEmailToken()
        {
            long userId = GetUserIdFromContext();
            AppUser appUser = await AppUserManager.FindByIdAsync(userId);
            if (appUser.EmailConfirmed)
            {
                return BadRequest(Responses.EmailAlreadyConfirmedResponseMessage);
            }

            var resultMessage = await SendEmailConfirmationAsync(appUser);

            // Configure reponse
            var response = new BaseResponseDto();
            response.Message = resultMessage;
            return Ok(response);
        }

        /// <summary>
        /// Confirms the email id for a newly created user using the code which was sent to his email id.
        /// Note: The GUI website must log in the user and then send the code.
        /// </summary>
        /// <param name="confirmEmailDto">The dto containing the confirmation code that was sent on the user's email id.</param>
        /// <returns></returns>
        [HttpPost]
        [ResponseType(typeof(ApiResponseDto<BaseResponseDto>))]
        public IHttpActionResult ConfirmEmail(ConfirmEmailReq confirmEmailDto)
        {
            long userId = GetUserIdFromContext();
            var code = HttpUtility.UrlDecode(confirmEmailDto.Code);
            IdentityResult identityResult = AppUserManager.ConfirmEmail(userId, code);
            if (identityResult.Succeeded)
            {
                var response = new BaseResponseDto();
                response.Message = Responses.EmailConfirmedMessage;
                return Ok(response);
            }

            return BadRequestWithIdentityErrors(identityResult);
        }

        /// <summary>
        /// Allows a logged in user to change the password.
        /// </summary>
        /// <param name="changePasswordDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ResponseType(typeof(ApiResponseDto<BaseResponseDto>))]
        public IHttpActionResult ChangePassword(ChangePasswordReq changePasswordDto)
        {
            long userId = GetUserIdFromContext();
            IdentityResult identityResult = AppUserManager.ChangePassword(changePasswordDto, userId);
            if (identityResult.Succeeded)
            {
                var response = new BaseResponseDto();
                response.Message = Responses.ChangePasswordResponseMessage;
                return Ok(response);
            }

            return BadRequestWithIdentityErrors(identityResult);
        }

        /// <summary>
        /// Generates a password reset token and sends it to the emailid of the user, if he is registered and his email id is confirmed.
        /// </summary>
        /// <param name="forgotPasswordDto"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ResponseType(typeof(ApiResponseDto<BaseResponseDto>))]
        // TODO: Apply throttling
        public async Task<IHttpActionResult> ForgotPassword(ForgotPasswordReq forgotPasswordDto)
        {
            var user = AppUserManager.FindByEmail(forgotPasswordDto.Email);
            if (user == null || !(AppUserManager.IsEmailConfirmed(user.Id)))
            {
                // Don't reveal that the user does not exist or is not confirmed
                return BadRequest();
            }

            var message = await SendForgotPasswordEmail(user);

            var response = new BaseResponseDto();
            response.Message = message;
            return Ok(response);
        }

        /// <summary>
        /// Receives the password reset token generated by ForgotPassword API. 
        /// It resets the user's password with the newly specified password.
        /// </summary>
        /// <param name="resetPasswordDto"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [System.Web.Mvc.ValidateAntiForgeryToken]
        [ResponseType(typeof(ApiResponseDto<BaseResponseDto>))]
        // TODO: Apply throttling
        public IHttpActionResult ResetPassword(ResetPasswordReq resetPasswordDto)
        {
            var user = AppUserManager.FindByEmail(resetPasswordDto.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return BadRequest();
            }
            var token = HttpUtility.UrlDecode(resetPasswordDto.PasswordResetToken);
            var identityResult = AppUserManager.ResetPassword(user.Id, token, resetPasswordDto.NewPassword);
            if (identityResult.Succeeded)
            {
                var response = new BaseResponseDto();
                response.Message = Responses.ResetPasswordResponseMessage;
                return Ok(response);
            }

            return BadRequestWithIdentityErrors(identityResult);
        }

        /// <summary>
        /// Gets the profile details of the user.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(ApiResponseDto<ProfileDto>))]
        public IHttpActionResult Profile()
        {
            var userId = GetUserIdFromContext();
            var dto = _accountService.GetProfile(userId);
            return Ok(dto);
        }

        /// <summary>
        /// Updates the profile details of the user.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ResponseType(typeof(ApiResponseDto<ProfileRes>))]
        public IHttpActionResult Profile(ProfileDto profileToUpdate)
        {
            var userId = GetUserIdFromContext();
            var dto = _accountService.UpdateProfile(userId, profileToUpdate);
            return Ok(dto);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// If the appuser has specified a phone number.
        /// Sends a SMS code to confirm the phone number of the user.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="registerPhoneNumberDto"></param>
        /// <returns></returns>
        private async Task SendPhoneNumberConfirmationAsync(long userId, RegisterPhoneNumberReq registerPhoneNumberDto)
        {
            if (!string.IsNullOrWhiteSpace(registerPhoneNumberDto.PhoneNumber))
            {
                var smsCode = await AppUserManager.GenerateChangePhoneNumberTokenAsync(userId, registerPhoneNumberDto.PhoneNumber);
                if (AppUserManager.SmsService != null)
                {
                    var message = new IdentityMessage
                    {
                        Destination = registerPhoneNumberDto.PhoneNumber,
                        Body = "Your Api security code is: " + smsCode
                    };
                    await AppUserManager.SmsService.SendAsync(message);
                }
            }
        }

        /// <summary>
        /// Sends a confirmation email to the user's email id. 
        /// The user must click it to validate his email Id.
        /// </summary>
        /// <param name="appUser"></param>
        /// <returns></returns>
        private async Task<string> SendEmailConfirmationAsync(AppUser appUser)
        {
            // Generate a Url encoded code/token
            string code = await AppUserManager.GenerateEmailConfirmationTokenAsync(appUser.Id);

            var guiWebsiteBaseUrl = ConfigurationManager.AppSettings[Constants.WebConfig.WebsiteBaseUrlKey];
            var guiWebsiteConfirmEmailPagePath = ConfigurationManager.AppSettings[Constants.WebConfig.ConfirmEmailPagePathKey];
            string encodedConfirmationCode = HttpUtility.UrlEncode(code);
            var callbackApiUrl = $"{guiWebsiteBaseUrl}{guiWebsiteConfirmEmailPagePath}?code={encodedConfirmationCode}";

            try
            {
                await AppUserManager.SendEmailAsync(appUser.Id, Emails.ConfirmEmailTitle, Emails.ConfirmEmailBodyPrefix + callbackApiUrl);
            }
            catch
            {
                return await Task.FromResult(Errors.FailedToSendConfirmationEmail);
            }
            return await Task.FromResult(Responses.SendConfirmEmailTokenResponseMessage);
        }

        private async Task<string> SendForgotPasswordEmail(AppUser user)
        {
            var passwordResetToken = AppUserManager.GeneratePasswordResetToken(user.Id);

            var guiWebsiteBaseUrl = ConfigurationManager.AppSettings[Constants.WebConfig.WebsiteBaseUrlKey];
            var guiWebsiteResetPasswordPagePath = ConfigurationManager.AppSettings[Constants.WebConfig.ResetPasswordPagePathKey];
            string encodedToken = HttpUtility.UrlEncode(passwordResetToken);
            var callbackApiUrl = $"{guiWebsiteBaseUrl}{guiWebsiteResetPasswordPagePath}?code={encodedToken}";
            try
            {
                await AppUserManager.SendEmailAsync(user.Id, Emails.ForgotPasswordTitle, Emails.ForgotPasswordBodyPrefix + callbackApiUrl);
            }
            catch
            {
                return await Task.FromResult(Errors.FailedToSendForgotPasswordEmail);
            }
            return await Task.FromResult(Responses.ForgotPasswordResponseMessage);
        }

        #endregion
    }
}