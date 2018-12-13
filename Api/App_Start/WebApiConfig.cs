using Api.App_Start;
using Api.Filters;
using Api.GlobalHandlers;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using WebApiThrottle;

namespace Api
{
    /// <summary>
    /// Configures the Web API and services
    /// </summary>
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Configure the Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Configure the Web API routes
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                  name: "ActionBasedRoutes",
                  routeTemplate: Constants.ApiRoutes.BaseApiPath.TrimStart('/') + "/{controller}/{action}/{id}",
                  defaults: new { id = RouteParameter.Optional }
            );

            // Convert the JSON Dtos to camelCase, which is JS friendly format
            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            // Register logging handler
            config.Services.Add(typeof(IExceptionLogger), new DatabaseExceptionLogger());

            // Replace Exception handler
            config.Services.Replace(typeof(IExceptionHandler), new GlobalExceptionHandler());

            // Add model validation filter
            config.Filters.Add(new ValidateModelAttribute());

            // Add Authorize attribute to all requests
            config.Filters.Add(new AuthorizeAttribute());

            // Mandates https for all requests
            config.MessageHandlers.Add(new RequireHttpsHandler());

            // Wrap al repsonses into a standard response
            config.MessageHandlers.Add(new ApiResponseWrapper());

            // Throttling configuration
            config.MessageHandlers.Add(new ThrottlingHandler()
            {
                Policy = ThrottlingConfig.GetDefaultPolicy(),
                Repository = new CacheRepository()
            });
        }
    }
}
