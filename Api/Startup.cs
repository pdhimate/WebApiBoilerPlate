using Microsoft.Owin;
using Owin;
using System.Net;

[assembly: OwinStartup(typeof(Api.Startup))]
namespace Api
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}