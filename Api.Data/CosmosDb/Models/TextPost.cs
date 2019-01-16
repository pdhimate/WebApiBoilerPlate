using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Data.CosmosDb.Models
{
    public class TextPost : CosmosModelBase
    {

        /// <summary>
        /// A general note describing the post.
        /// </summary>
        public string Note { get; set; }


        public long CreatedByUserId { get; set; }

        public string CreatedByUserName { get; set; }

        // Any other properties that you may want to add
    }
}
