using Api.Data;
using Api.Data.Access.Repositories;
using Api.Data.Access.Repositories.Meta;
using Api.Data.Models.Meta;
using System;

namespace Api.Services.Logging
{
    /// <summary>
    /// Provides for logging the LogEntries into a database
    /// </summary>
    public class DatabaseLoggerService : ILoggerService
    {
        private readonly AppDatabaseContext _context;

        private ILogEntryRepository _logEntires;
        public ILogEntryRepository LogEntires
        {
            get
            {
                if (_logEntires == null)
                {
                    _logEntires = new LogEntryRepository(_context);
                }
                return _logEntires;
            }
        }

        public DatabaseLoggerService()
        {
            _context = new AppDatabaseContext();
        }

        #region ILogger implementation

        public void Log(LogEntry logEntry)
        {
            LogEntires.Insert(logEntry);
            _context.SaveChangesAsync();
        }

        #region IDisposable implementation

        private bool disposed = false;

        /// <summary>  
        /// Protected Virtual Dispose method  
        /// </summary>  
        /// <param name="disposing"></param>  
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            disposed = true;
        }

        /// <summary>  
        /// Dispose method  
        /// </summary>  
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #endregion
    }
}