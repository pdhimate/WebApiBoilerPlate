using Microsoft.Practices.Unity;

namespace DependencyResolver
{
    /// <summary>
    /// A wrapper class over MEF and Unity to register types/components
    /// </summary>
    internal class RegisterComponent : IRegisterComponent
    {
        private readonly IUnityContainer _container;

        public RegisterComponent(IUnityContainer container)
        {
            _container = container;
            //Register interception behaviour if any  
        }

        public void RegisterType<TFrom, TTo>(bool withInterception = false) where TTo : TFrom
        {
            if (withInterception)
            {
                //register with interception   
            }
            else
            {
                _container.RegisterType<TFrom, TTo>();
            }
        }

        public void RegisterTypeWithControlledLifeTime<TFrom, TTo>(bool withInterception = false) where TTo : TFrom
        {
            _container.RegisterType<TFrom, TTo>(new ContainerControlledLifetimeManager());
        }

        public void RegisterTypeWithTransientLifetime<TFrom, TTo>(bool withInterception = false) where TTo : TFrom
        {
            _container.RegisterType<TFrom, TTo>(new TransientLifetimeManager());
        }
    }
}
