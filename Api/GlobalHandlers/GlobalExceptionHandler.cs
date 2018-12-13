using Api.BusinessEntities;
using Api.BusinessEntities.Common;
using Api.BusinessService;
using Api.Resources;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Results;

namespace Api.GlobalHandlers
{
    public class GlobalExceptionHandler : ExceptionHandler
    {
        public override void Handle(ExceptionHandlerContext context)
        {
            // Note: The errorId should be popluated by the ExceptionLogger.
            var errorId = (context.Exception.Data[Constants.ExceptionCustomDataKeys.ErrorReferenceKey] as string) ?? string.Empty;
            var notLoggedError = (context.Exception.Data[Constants.ExceptionCustomDataKeys.LoggingFailureKey] as string) ?? string.Empty;

            // Create errorDto
            var errorDto = new ErrorDto
            {
                Title = Errors.DefaultErrorMessage,
                ErrorReference = errorId,
                Messages = new List<string>(),
            };

            if (!string.IsNullOrWhiteSpace(notLoggedError))
            {
                errorDto.Messages.Add(notLoggedError);
            }

            // If business exception has been thrown, add the corresponding error message to the errorDto
            if (context.Exception is BusinnessException)
            {
                var businnessException = context.Exception as BusinnessException;
                errorDto.Title = Errors.BusinessErrorMessageTitle;
                errorDto.Messages.Add(businnessException.Message);
            }

            // Create api response
            var apiResponseDto = new ApiResponseDto<object>
            {
                Error = errorDto,
                Result = null,
            };

            // Create http response
            var response = context.Request.CreateResponse(HttpStatusCode.InternalServerError, apiResponseDto);
            context.Result = new ResponseMessageResult(response);
        }
    }
}