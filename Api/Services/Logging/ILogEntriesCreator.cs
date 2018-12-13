using Api.Data.Models.Meta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Api.Services.Logging
{
    /// <summary>
    /// Defines the methods required for creating a logging entries : <see cref="LogEntry"/>
    /// </summary>
    public interface ILogEntriesCreator
    {
        /// <summary>
        /// Creates a HttpRequestMessageEntry object from the specified HttpRequestMessage, which can be logged.
        /// </summary>
        /// <param name="httpRequestMessage"></param>
        /// <returns></returns>
        HttpRequestMessageEntry CreateHttpRequestEntry(HttpRequestMessage httpRequestMessage);

        /// <summary>
        /// Creates an ExceptionEntry object from the specified Excpetion, which can be logged.
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        ExceptionEntry CreateExceptionEntry(Exception exception);
    }
}
