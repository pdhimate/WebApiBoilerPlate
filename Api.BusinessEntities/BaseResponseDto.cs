using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.BusinessEntities
{
    /// <summary>
    /// Wraps the common properties required by most of the ResponseDtos
    /// </summary>
    public class BaseResponseDto
    {
        /// <summary>
        /// Represents a userfriendly message.
        /// </summary>
        public string Message { get; set; }

    }
}
