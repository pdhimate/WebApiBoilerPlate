using Api.BusinessEntities;
using Api.BusinessEntities.AccountController;
using Api.Data.Models;
using Api.Data.Models.Security;
using Microsoft.AspNet.Identity;
using System;

namespace Api.Auth
{
    /// <summary>
    /// Provides methods to Register new user, Change Password.
    /// These methods require an instance of <see cref="AppUserManager"/> from Owin context of the request, hence are generally extension methods.
    /// The methods are related to authentication, which is a responsibility of the API and not the Business logic.
    /// </summary>
    public static class AppUserManagerExtensions
    {
        /// <summary>
        /// Register a new user.
        /// </summary>
        /// <param name="appUserManager">The instance of the <see cref="AppUserManager"/> from the Owin context of the request.</param>
        /// <param name="registerUserDto"></param>
        /// <returns>IdentityResult</returns>
        public static IdentityResult RegisterUser(this AppUserManager appUserManager, RegisterUserReq registerUserDto)
        {
            var appUser = new AppUser
            {
                UserName = string.IsNullOrWhiteSpace(registerUserDto.UserName) ? registerUserDto.Email : registerUserDto.UserName,
                Email = registerUserDto.Email,
                CreatedOnUtc = DateTime.UtcNow,
                TwoFactorEnabled = registerUserDto.TwoFactorEnabled,
            };

            // If two factor authentication has been enabled, generate a private shared key for it.
            if (registerUserDto.TwoFactorEnabled)
            {
                appUser.TwoFactorPreSharedKey = TimeSensitivePassCode.GenerateSharedPrivateKey();
            }


            var identityResult = appUserManager.Create(appUser, registerUserDto.Password);
            return identityResult;
        }


        public static IdentityResult ChangePassword(this AppUserManager appUserManager, ChangePasswordReq changePasswordDto, long userId)
        {
            IdentityResult identityResult = appUserManager.ChangePassword(userId,
                                                               changePasswordDto.OldPassword,
                                                               changePasswordDto.NewPassword);
            return identityResult;
        }


    }
}