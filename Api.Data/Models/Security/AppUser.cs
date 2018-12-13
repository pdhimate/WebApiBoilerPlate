using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Data.Models.Security
{
    public class AppUser : IdentityUser<long, ExternalUserLogin, AppUserAppRoleMapping, AppUserClaim>, IUser<long>
    {
        public DateTime? CreatedOnUtc { get; set; }

        #region Name

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        /// <summary>
        /// The private secret key for two factor authentication.
        /// </summary>
        [MaxLength(16)]
        public string TwoFactorPreSharedKey { get; set; }

        #endregion

        #region Foreign keys: Address

        /// <summary>
        /// The Id corresponding to the default geographical address of this user.
        /// </summary>
        public long? DefaultAddressId { get; set; }

        /// <summary>
        /// The geographical Address of this user.
        /// </summary>
        [ForeignKey("DefaultAddressId")]
        public virtual Address Address { get; set; }

        #endregion
    }
}
