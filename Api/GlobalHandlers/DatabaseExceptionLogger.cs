using Api.Data.Models.Meta;
using System;
using System.Web.Http.ExceptionHandling;
using Api.Services.Logging;

namespace Api.GlobalHandlers
{
    /// <summary>
    /// Logs exceptions into the database
    /// </summary>
    public class DatabaseExceptionLogger : ExceptionLogger, IDisposable
    {
        #region Fields

        private readonly ILoggerService _loggerService;
        private readonly ILogEntriesCreator _logEntryCreator;

        #endregion

        public DatabaseExceptionLogger() : this(null, null)
        {
        }

        public DatabaseExceptionLogger(ILoggerService logger, ILogEntriesCreator logEntryCreator) : base()
        {
            _loggerService = logger ?? new DatabaseLoggerService();
            _logEntryCreator = logEntryCreator ?? new LogEntriesCreator();
        }

        #region Public Overrides

        public override void Log(ExceptionLoggerContext context)
        {
            var exceptionEntry = _logEntryCreator.CreateExceptionEntry(context.Exception);
            var httpRequestEntry = _logEntryCreator.CreateHttpRequestEntry(context.Request);
            var logEntry = new LogEntry(exceptionEntry, httpRequestEntry);
            try
            {
                _loggerService.Log(logEntry);
            }
            catch (Exception ex)
            {
                // TODO: Explore other options to log here. 
                // Perhaps send an email to admin? 
                var errors = ex.Message;
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                    errors += ex.Message;
                }
                context.Exception.Data.Add(Constants.ExceptionCustomDataKeys.LoggingFailureKey, $"Failed to create a log entry. {ex.Message}.");
            }

            // Update the excetion.Data with the reference of the logged Id. 
            // Note: This would be sent to the support by the end users, when they see an error.
            context.Exception.Data.Add(Constants.ExceptionCustomDataKeys.ErrorReferenceKey, logEntry.Id);

            base.Log(context);
        }

        #endregion

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
                    _loggerService.Dispose();
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

    }
}