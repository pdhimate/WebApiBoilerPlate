using Api.Data.Models;
using Api.Data.Models.Security;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Api.Auth.Validators
{
    /// <summary>
    /// Configures validation logic for user registrations.
    /// </summary>
    public class AppUserValidator : UserValidator<AppUser, long>
    {
        /// <summary>
        /// Allow user registrations only from these domains.
        /// Some domains provide temporary email ids, which perish after some time. We do not want users to use them.
        /// </summary>
        public readonly HashSet<string> AllowedEmailDomains = new HashSet<string>
        {
            "outlook.com", "live.com", "hotmail.com", "microsoft.com",
            "gmail.com","google.com",
            "yahoo.com","yahoo.co.in", "ymail.com",
            "facebook.com"
        };

        public AppUserValidator(AppUserManager appUserManager)
            : base(appUserManager)
        {
            AllowOnlyAlphanumericUserNames = true;
            RequireUniqueEmail = true;
        }

        public override async Task<IdentityResult> ValidateAsync(AppUser user)
        {
            IdentityResult result = await base.ValidateAsync(user);

            var emailDomain = user.Email.Split('@')[1];

            if (!AllowedEmailDomains.Contains(emailDomain.ToLower()))
            {
                var errors = result.Errors.ToList();

                errors.Add(string.Format("Email domain '{0}' is not allowed", emailDomain));

                result = new IdentityResult(errors);
            }

            return result;
        }
    }
}