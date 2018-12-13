using Api.BusinessEntities;
using Api.BusinessEntities.Common;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Api.GlobalHandlers
{
    /// <summary>
    /// Applying this handler requires requests to be sent using HTTPS.
    /// If the request was not sent via HTTPS this handler returns a body explaining where to find the requested resource and automatically redirects the caller if possible.
    /// </summary>
    public class RequireHttpsHandler : DelegatingHandler
    {
        private readonly int _httpsPort;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequireHttpsHandler" /> class.
        /// </summary>
        public RequireHttpsHandler()
            : this(443)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequireHttpsHandler" /> class.
        /// </summary>
        /// <param name="httpsPort">The HTTPS port.</param>
        public RequireHttpsHandler(int httpsPort)
        {
            _httpsPort = httpsPort;
        }

        /// <summary>
        /// Sends an HTTP request to the inner handler to send to the server as an asynchronous operation.
        /// </summary>
        /// <param name="request">The HTTP request message to send to the server.</param>
        /// <param name="cancellationToken">A cancellation token to cancel operation.</param>
        /// <returns>Returns <see cref="T:System.Threading.Tasks.Task`1" />. The task object representing the asynchronous operation.</returns>
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.RequestUri.Scheme == Uri.UriSchemeHttps)
            {
                return base.SendAsync(request, cancellationToken);
            }

            var response = CreateResponse(request);
            var tcs = new TaskCompletionSource<HttpResponseMessage>();
            tcs.SetResult(response);
            return tcs.Task;
        }

        /// <summary>
        /// Creates the response based on the request method.
        /// </summary>
        /// <remarks>
        /// Based on http://blogs.msdn.com/b/carlosfigueira/archive/2012/03/09/implementing-requirehttps-with-asp-net-web-api.aspx
        /// </remarks>
        /// <param name="request">The request.</param>
        /// <returns>The response based on the request method.</returns>
        private HttpResponseMessage CreateResponse(HttpRequestMessage request)
        {
            var uriBuilder = new UriBuilder(request.RequestUri);
            uriBuilder.Scheme = Uri.UriSchemeHttps;
            uriBuilder.Port = _httpsPort;

            // Create errorDto
            var errorDto = new ErrorDto
            {
                Title = $"HTTPS is required. The resource can be found at {uriBuilder.Uri.AbsoluteUri}",
                ErrorReference = Guid.Empty.ToString(),
            };

            // Create api response
            var apiResponseDto = new ApiResponseDto<object>
            {
                Error = errorDto,
                Result = null,
            };

            // Create http response
            HttpResponseMessage response = request.CreateResponse(HttpStatusCode.Forbidden, apiResponseDto);
            return response;
        }
    }
}