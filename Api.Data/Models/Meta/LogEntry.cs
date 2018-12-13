using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Api.Data.Models.Meta
{
    [Table("LogEntries", Schema = Constants.DatabaseSchemas.MetaDataSchemaName)]
    public class LogEntry : BaseEntry
    {
        public LogEntry(ExceptionEntry exceptionEntry, HttpRequestMessageEntry httpRequestMessageEntry)
        {
            CreatedOnUtc = DateTime.UtcNow;
            MachineName = Environment.MachineName;

            ExceptionEntry = exceptionEntry;
            HttpRequestMessageEntry = httpRequestMessageEntry;
        }

        #region Properties

        public DateTime CreatedOnUtc { get; private set; }

        /// <summary>
        /// The name of the computer on which the assmebly was running.
        /// </summary>
        public string MachineName { get; private set; }

        #endregion

        #region Foreign Keys
        /// <summary>
        /// The Id corresponding to the exception for this log entry.
        /// </summary>
        public string ExceptionId { get; set; }

        [ForeignKey("ExceptionId")]
        public ExceptionEntry ExceptionEntry { get; private set; }

        /// <summary>
        /// The Id corresponding to the HttpRequest for this log entry.
        /// </summary>
        public string HttpRequestId { get; set; }

        [ForeignKey("HttpRequestId ")]
        public HttpRequestMessageEntry HttpRequestMessageEntry { get; private set; }

        #endregion
    }
}
