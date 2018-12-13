using Api.Data.Models.Meta;
using System;

namespace Api.Services.Logging
{
    /// <summary>
    /// Provides an interface to log expections and log entries
    /// </summary>
    public interface ILoggerService : IDisposable
    {
        /// <summary>
        /// Synchronously saves the log entry in to the database 
        /// </summary>
        /// <param name="logEntry"></param>
        void Log(LogEntry logEntry);
    }
}