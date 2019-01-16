using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.BusinessEntities.PostsController
{
    /// <summary>
    /// Create TextPost request
    /// </summary>
    public class CreateTextPostReq
    {
        /// <summary>
        /// A general note describing the post.
        /// </summary>
        public string Note { get; set; }
    }

    /// <summary>
    /// TextPost response
    /// </summary>
    public class TextPostRes : CreateTextPostReq
    {
        public string Id { get; set; }

        public DateTime CreatedOnUtc { get; set; }

        public string CreatedByUserName { get; set; }
    }
}
