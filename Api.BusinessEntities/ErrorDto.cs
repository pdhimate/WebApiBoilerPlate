using System;
using System.Collections.Generic;

namespace Api.BusinessEntities
{
    /// <summary>
    /// An object of this class would be sent as a response, when as exception is thrown or an error occurs.
    /// </summary>
    public class ErrorDto
    {
        /// <summary>
        /// The UTC when this exception was being handled.
        /// </summary>
        public DateTime DateTimeUtc { get; set; }

        /// <summary>
        /// The Id that has been logged by the ErrorLogger. The end user may send this to customer care. This is used to search the LogEntries table.
        /// An Guid.Empty value indicates that there was no exception thrown by ErrorLogger.
        /// </summary>
        public string ErrorReference { get; set; }

        /// <summary>
        /// Userfriendly error title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Userfriendly error messages.
        /// </summary>
        public List<string> Messages { get; set; }

        public ErrorDto()
        {
            DateTimeUtc = DateTime.UtcNow;
        }
    }
}
