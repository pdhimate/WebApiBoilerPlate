using System;

namespace DependencyResolver
{
    /// <summary>  
    /// Acts as a Contract that allows any assembly to register its Dependency types with Unity.  
    /// </summary>  
    public interface IComponent
    {
        void Setup(IRegisterComponent registerComponent);
    }
}
