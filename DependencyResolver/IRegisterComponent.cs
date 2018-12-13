using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyResolver
{
    /// <summary>  
    /// Responsible for registering types in unity configuration by implementing IComponent  
    /// </summary>  
    public interface IRegisterComponent
    {
        /// <summary>  
        /// Register type method  
        /// </summary>  
        /// <typeparam name="TFrom"></typeparam>  
        /// <typeparam name="TTo"></typeparam>  
        /// <param name="withInterception"></param>  
        void RegisterType<TFrom, TTo>(bool withInterception = false) where TTo : TFrom;

        /// <summary>  
        /// Register type with container controlled life time manager.
        /// Returns the same instance of the registered type or object each time you Resolve or inject the dependency.
        /// </summary>  
        /// <typeparam name="TFrom"></typeparam>  
        /// <typeparam name="TTo"></typeparam>  
        /// <param name="withInterception"></param>  
        void RegisterTypeWithControlledLifeTime<TFrom, TTo>(bool withInterception = false) where TTo : TFrom;

        /// <summary>  
        /// Register type with TransientLifetime life time manager.
        /// Provides a new instance per call. 
        /// You can call Resolve within your worker thread, use the instance, and dispose of it before the method exits.
        /// </summary>  
        /// <typeparam name="TFrom"></typeparam>  
        /// <typeparam name="TTo"></typeparam>  
        /// <param name="withInterception"></param>  
        void RegisterTypeWithTransientLifetime<TFrom, TTo>(bool withInterception = false) where TTo : TFrom;
    }
}
