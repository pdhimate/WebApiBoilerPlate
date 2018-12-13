using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Data.Models.Meta
{
    [Table("ExceptionEntries", Schema = Constants.DatabaseSchemas.MetaDataSchemaName)]
    public class ExceptionEntry : BaseEntry
    {
        #region Properties

        /// <summary>
        /// A coded numerical value that is assigned to a specific exception
        /// </summary>
        public int HResult { get; set; }

        /// <summary>
        /// Gets a message that describes the current exception.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the name of the application or the object that causes the error.
        /// </summary>
        public string Source { get; set; }

        public string StackTrace { get; set; }

        /// <summary>
        /// Gets the method that throws the current exception.
        /// </summary>
        public string Method { get; set; }

        #endregion

        #region Foreign keys

        /// <summary>
        /// The Id corresponding to the inner exception for this expcetion entry.
        /// </summary>
        public string InnnerExceptionId { get; set; }

        [ForeignKey("InnnerExceptionId")]
        public virtual ExceptionEntry InnerException { get; set; }

        #endregion
    }
}
