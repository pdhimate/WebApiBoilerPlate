using Api.Data.Models.Meta;
using Api.GlobalHandlers;
using Api.Services.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Net.Http;
using System.Web.Http.ExceptionHandling;

namespace Api.UnitTests.GlobalHandlers
{
    [TestFixture(Category = TestHelper.Constants.TestCategories.UnitTests)]
    [Parallelizable(ParallelScope.Children)]
    public class DatabaseExceptionLoggerTests
    {
        [Test]
        public void Log_AddsErrorReferenceGuid_InExceptionData()
        {
            var logEntryCreatorMock = new Mock<ILogEntriesCreator>();
            logEntryCreatorMock.Setup(lec => lec.CreateExceptionEntry(It.IsAny<Exception>())).Returns(new ExceptionEntry());
            logEntryCreatorMock.Setup(lec => lec.CreateHttpRequestEntry(It.IsAny<HttpRequestMessage>())).Returns(new HttpRequestMessageEntry());

            var loggerServiceMock = new Mock<ILoggerService>();
            loggerServiceMock.Setup(ls => ls.Log(It.IsAny<LogEntry>()));

            var dbLoggerMock = new DatabaseExceptionLogger(loggerServiceMock.Object, logEntryCreatorMock.Object);

            var exception = new Exception("Some Exception occurred!");
            var exceptionContext = new ExceptionContext(exception, new ExceptionContextCatchBlock("name", true, true));
            var exceptionLoggerContextMock = new ExceptionLoggerContext(exceptionContext);

            dbLoggerMock.Log(exceptionLoggerContextMock);

            // Must contain error reference key
            Assert.IsTrue(exceptionContext.Exception.Data.Contains(Constants.ExceptionCustomDataKeys.ErrorReferenceKey));
            var referenceKey = exceptionContext.Exception.Data[Constants.ExceptionCustomDataKeys.ErrorReferenceKey].ToString();

            // Must be a Guid
            Guid guid;
            Assert.IsTrue(Guid.TryParse(referenceKey, out guid));
        }
    }
}