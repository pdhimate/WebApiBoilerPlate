using Microsoft.ApplicationInsights.Extensibility;
using System.Diagnostics;
using System.Web.Http;
using System.Web.Mvc;

namespace Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            // This enables the HelpController under the Areas folder.
            AreaRegistration.RegisterAllAreas();

            // Use UnityContainer for Dependency Injection and Inversion of control.
            UnityConfig.RegisterComponents();

            DisableApplicationInsightsOnDebug();
        }

        /// <summary>
        /// Disables the application insights locally.
        /// </summary>
        [Conditional("DEBUG")]
        private static void DisableApplicationInsightsOnDebug()
        {
            TelemetryConfiguration.Active.DisableTelemetry = true;
        }
    }
}
