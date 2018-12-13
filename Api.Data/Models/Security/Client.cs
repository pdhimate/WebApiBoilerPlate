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
    [Table("Clients", Schema = Constants.DatabaseSchemas.SecurityDataSchemaName)]
    public class Client : IBaseModel<string>
    {
        /// <summary>
        /// Primary key.
        /// Usualy short name of the client.
        /// </summary>
        [Key]
        [MaxLength(50)]
        public string Id { get; set; }

        /// <summary>
        /// Hashed - client secret.
        /// </summary>
        public string Secret { get; set; }

        /// <summary>
        /// The user friendly name for the client.
        /// e.g: console application, Angular JS app.
        /// </summary>
        [MaxLength(100)]
        public string Name { get; set; }

        public ClientType ClientType { get; set; }

        /// <summary>
        /// Set this false to disable any requests for access tokens from the client.
        /// In short, disable the access for the client.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// The life span of a refresh token in minutes
        /// </summary>
        public long RefreshTokenLifeTime { get; set; }

        /// <summary>
        /// Used to configure client specific CORS.
        /// E.G: For a JS website this value should be the url of the website host. 
        /// This way nobody else can create a replica of the website to make calls
        /// </summary>
        [MaxLength(200)]
        public string AllowedOrigin { get; set; }
    }

    public enum ClientType
    {
        /// <summary>
        /// E.g Website
        /// </summary>
        JavaScript = 0,
        
        /// <summary>
        /// e.g Andoird app, etc
        /// </summary>
        NativeConfidential = 1 
    }
}
