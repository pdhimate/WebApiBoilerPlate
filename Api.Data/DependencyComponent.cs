using Api.Data.Access;
using DependencyResolver;
using System.ComponentModel.Composition;

namespace Api.Data
{
    /// <summary>
    /// Exposes the dependencies made available by this project. 
    /// Indicates to the MEF that these components need to be loaded at runtime.
    /// </summary>
    [Export(typeof(IComponent))]
    public class DependencyComponent : IComponent
    {
        public void Setup(IRegisterComponent registerComponent)
        {
            registerComponent.RegisterTypeWithTransientLifetime<IUnitOfWork, UnitOfWork>();
        }
    }
}
