using DependencyResolver;
using Microsoft.Practices.Unity;
using System.Web.Http;
using Unity.WebApi;

namespace Api
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            // Note: All the dependencies have been specified as IComponent to MEF by respective projects. 
            // Hence no need to register any types here.
            // Instead we specify the various assemblies which contain the Components required to resolve the dependencies or inject them.  
            ComponentLoader.LoadContainer(container, ".\\bin", "Api.BusinessEntities.dll");
            ComponentLoader.LoadContainer(container, ".\\bin", "Api.BusinessService.dll");
            ComponentLoader.LoadContainer(container, ".\\bin", "Api.Data.dll");

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}