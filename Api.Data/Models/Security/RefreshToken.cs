using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Data.Models.Security
{
    /// <summary>
    /// Refer: http://bitoftech.net/2014/07/16/enable-oauth-refresh-tokens-angularjs-app-using-asp-net-web-api-2-owin/
    /// </summary>
    [Table("RefreshTokens", Schema = Constants.DatabaseSchemas.SecurityDataSchemaName)]
    public class RefreshToken : IBaseModel<string>
    {
        /// <summary>
        /// Primary key. GUID.
        /// </summary>
        [Key]
        public string Id { get; set; }

        /// <summary>
        /// Display purpose only.
        /// </summary>
        public DateTime IssuedOnUtc { get; set; }

        /// <summary>
        /// Display purpose only.
        /// </summary>
        public DateTime ExpiresOnUtc { get; set; }

        /// <summary>
        /// Contains magical signed string which contains a serialized representation for the ticket for specific user.
        /// In other words it contains all the claims and ticket properties for this user.
        /// Owin middle-ware will use this string to build a new access token
        /// </summary>
        [Required]
        public string ProtectedTicket { get; set; }

        #region Foreign keys

        [Required]
        public string ClientId { get; set; }

        [ForeignKey(nameof(ClientId))]
        public virtual Client Client { get; set; }

        public long UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual AppUser User { get; set; }

        #endregion
    }
}
