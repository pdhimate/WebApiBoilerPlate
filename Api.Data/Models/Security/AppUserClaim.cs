using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;

namespace Api.Data.Models.Security
{
    /// <summary>
    /// A Claim is a statement about the user makes about itself. 
    /// It can be user name, user id, gender, phone, the roles user assigned to, etc.
    /// This information can be used to restrict the user to alter, only the data that belongs to him. 
    /// E.g a user may only change the Address that belongs to him.
    /// </summary>
    public class AppUserClaim : IdentityUserClaim<long>
    {
        [ForeignKey(nameof(IdentityUserClaim.UserId))]
        public virtual AppUser AppUser { get; set; }
    }

    /// <summary>
    /// The claims that can be used within the application, apart from the default<see cref="ClaimTypes"/>.
    /// </summary>
    public class AppUserClaims
    {
        public static List<string> GetAll()
        {
            return new List<string>
            {
                TwoFactorPskClaimKey
            };
        }

        /// <summary>
        /// Every user who has two factor authorization enabled, can use this claim to login.
        /// </summary>
        public static string TwoFactorPskClaimKey { get { return "PSK"; } }
    }

}
