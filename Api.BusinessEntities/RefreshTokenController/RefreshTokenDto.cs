using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.BusinessEntities.RefreshTokenController
{
    public class RefreshTokenRes
    {
        /// <summary>
        /// Encrypted token value as the Id
        /// </summary>
        public string Id { get; set; }

        public DateTime IssuedOnUtc { get; set; }

        public DateTime ExpiresOnUtc { get; set; }

        /// <summary>
        /// The client name, that was used to generate this token
        /// </summary>
        public string ClientName { get; set; }

        /// <summary>
        /// The user to which this token belongs to
        /// </summary>
        public string UserName { get; set; }
    }
}
