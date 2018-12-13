using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.BusinessEntities.RefreshTokenController
{
    public class DeleteRefreshTokenReq
    {
        /// <summary>
        /// The id of the token to be deleted
        /// </summary>
        public string Id { get; set; }
    }
}
