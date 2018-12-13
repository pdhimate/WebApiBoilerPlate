using Api.BusinessEntities;
using Api.BusinessEntities.Common;
using Api.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Api.GlobalHandlers
{
    /// <summary>
    /// Wraps the HttpResponseMessage objects from the API into ApiResponseDto
    /// </summary>
    public class ApiResponseWrapper : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);
            return WrapApiResponse(request, response);
        }

        #region Private methods

        private static HttpResponseMessage WrapApiResponse(HttpRequestMessage request, HttpResponseMessage response)
        {
            object content;
            ErrorDto errorDto = null;

            // Extract content object. Check if it contains an HttpError, if yes populate the errorDto object.
            // In case of IdentityResult errors returned by BaseApiController.GetResultWithErrors(), content is HttpError
            if (response.TryGetContentValue(out content) && !response.IsSuccessStatusCode)
            {
                var httpError = content as HttpError;
                if (httpError != null)
                {
                    content = null; // Since have an error, no need to return any content.
                    var errors = httpError.Message.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                                                  .ToList();
                    errorDto = new ErrorDto
                    {
                        Title = Errors.ValidationErrorMessage,
                        Messages = errors,
                        ErrorReference = Guid.Empty.ToString(),
                    };
                }
                else if ((int)response.StatusCode == 429 // Too many requests. HttpStatusCode enum is unavailable at the moment
                       && content is string
                       && ((string)content).StartsWith("API calls quota exceeded!"))
                {
                    errorDto = new ErrorDto
                    {
                        Title = Errors.TooManyRequestErrorTitle,
                        Messages = new List<string> { (string)content },
                        ErrorReference = Guid.Empty.ToString(),
                    };
                    content = null;
                }
            }

            // Check if the response contains ErrorDto, if yes populate the errorDto object.
            // In case of ModelValidation errors, content is ErrorDto.
            if (content is ErrorDto)
            {
                errorDto = content as ErrorDto;
                content = null;
            }

            // Wrap the response/result
            var apiResponseDto = new ApiResponseDto<object>
            {
                Error = errorDto,
                Result = content,
            };

            // Create Http response with the wrapped result
            HttpResponseMessage wrappedResponse = CreateHttpResponseMessage(request, response, apiResponseDto);
            return wrappedResponse;
        }

        private static HttpResponseMessage CreateHttpResponseMessage(HttpRequestMessage request,
                                                                     HttpResponseMessage response,
                                                                     ApiResponseDto<object> apiResponseDto)
        {
            HttpResponseMessage wrappedResponse = request.CreateResponse(response.StatusCode, apiResponseDto);
            wrappedResponse.StatusCode = response.StatusCode;
            wrappedResponse.ReasonPhrase = response.ReasonPhrase;
            foreach (var header in response.Headers)
            {
                wrappedResponse.Headers.Add(header.Key, header.Value);
            }

            return wrappedResponse;
        }

        #endregion
    }
}