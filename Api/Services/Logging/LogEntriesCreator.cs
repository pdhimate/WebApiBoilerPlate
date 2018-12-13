using System;
using System.Linq;
using System.Net.Http;
using System.Web;
using Api.Data.Models.Meta;
using System.Net.Http.Headers;

namespace Api.Services.Logging
{
    /// <summary>
    /// Creates LogEntry based upon an Exception and/or HttpRequest.
    /// </summary>
    public class LogEntriesCreator : ILogEntriesCreator
    {
        #region Constants

        public const string HttpContextKey = "MS_HttpContext";
        public const string HttpOwinContextKey = "MS_OwinContext";

        #endregion

        #region ILogEntryCreator implementation

        public HttpRequestMessageEntry CreateHttpRequestEntry(HttpRequestMessage httpRequestMessage)
        {
            if (httpRequestMessage == null)
            {
                return null;
            }

            var httpRequestMessageEntry = new HttpRequestMessageEntry
            {
                Method = httpRequestMessage.Method.Method,
                RequestOriginalUri = httpRequestMessage.RequestUri.OriginalString,
                IdnHost = httpRequestMessage.RequestUri.IdnHost,
                Headers = ObfuscateSensitiveData(httpRequestMessage.Headers),
                UserName = ExtractUserName(httpRequestMessage),
                IpAddress = ExtractIpAddress(httpRequestMessage),
            };
            return httpRequestMessageEntry;
        }

        public ExceptionEntry CreateExceptionEntry(Exception exception)
        {
            if (exception == null)
            {
                return null;
            }
            var exceptionEntry = new ExceptionEntry();
            exceptionEntry.HResult = exception.HResult;
            if (exception.InnerException != null)
            {
                exceptionEntry.InnerException = CreateExceptionEntry(exception.InnerException);
            }
            exceptionEntry.Message = exception.Message;
            exceptionEntry.Source = exception.Source;
            exceptionEntry.StackTrace = exception.StackTrace;
            if (exception.TargetSite != null)
            {
                exceptionEntry.Method = exception.TargetSite.Name; // Method name
            }

            return exceptionEntry;
        }

        #endregion

        #region Private helper methods

        private string ExtractIpAddress(HttpRequestMessage httpRequestMessage)
        {
            if (!httpRequestMessage.Properties.ContainsKey(HttpOwinContextKey))
            {
                return null;
            }

            dynamic httpContextWrapper = httpRequestMessage.Properties[HttpOwinContextKey];
            if (httpContextWrapper != null && httpContextWrapper.Request != null)
            {
                return httpContextWrapper.Request.RemoteIpAddress;
            }

            return null;
        }

        private static string ExtractUserName(HttpRequestMessage httpRequestMessage)
        {
            if (!httpRequestMessage.Properties.ContainsKey(HttpContextKey))
            {
                return null;
            }

            var httpContextWrapper = httpRequestMessage.Properties[HttpContextKey] as HttpContextWrapper;
            if (httpContextWrapper == null)
            {
                return null;
            }

            var userName = httpContextWrapper.User?.Identity?.Name;
            return userName;
        }

        /// <summary>
        /// Replaces sensitive data with a dummy data.
        /// </summary>
        /// <param name="headers"></param>
        /// <returns></returns>
        private static string ObfuscateSensitiveData(HttpRequestHeaders headers)
        {
            const string defaultHeaderSeparator = "\r\n";

            var tokens = headers.ToString().Split(defaultHeaderSeparator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            var filteredHeaders = tokens.Select(header =>
            {
                // Replace the access token
                if (header.StartsWith("Authorization: Bearer "))
                {
                    return "Authorization: Bearer xxxxxxxxx";
                }
                return header;
            });
            return string.Join(defaultHeaderSeparator, filteredHeaders);
        }

        #endregion
    }
}