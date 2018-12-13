using Api.Auth.Validators;
using Api.Data;
using Api.Data.Models;
using Api.Data.Models.Security;
using Api.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Api.Auth
{
    /// <summary>
    /// Manages Users in context of OWIN, to configure authenticaion.
    /// </summary>
    public class AppUserManager : UserManager<AppUser, long>
    {
        public AppUserManager(IUserStore<AppUser, long> userStore) : base(userStore)
        {
        }

        #region Public static methods

        public static AppUserManager CreateUserManager(IdentityFactoryOptions<AppUserManager> options, IOwinContext context)
        {
            var appDbContext = context.Get<AppDatabaseContext>();
            var appUserManager = new AppUserManager(new ApplicationUserStore(appDbContext));

            // Configure validators
            appUserManager.UserValidator = new AppUserValidator(appUserManager);
            appUserManager.PasswordValidator = new AppPasswordValidator();

            // TODO: find out what this does
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                // TODO: Explore what ASP.NET Identity means, below
                appUserManager.UserTokenProvider = new DataProtectorTokenProvider<AppUser, long>(dataProtectionProvider.Create("ASP.NET Identity"));
            }

            // Configure email service provider
            appUserManager.EmailService = new SendgridEmailService();

            // Configure sms service provider
            appUserManager.SmsService = new TwilioSmsService();

            return appUserManager;
        }

        public static async Task<ClaimsIdentity> GenerateUserIdentityAsync(AppUser appUser, AppUserManager manager, string authenticationType)
        {
            // Note: The authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(appUser, authenticationType);

            // Note: Add custom user claims here


            return userIdentity;
        }

        #endregion
    }
}