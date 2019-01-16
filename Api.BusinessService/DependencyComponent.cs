using Api.BusinessService.Admin;
using Api.BusinessService.Common;
using DependencyResolver;
using System.ComponentModel.Composition;

namespace Api.BusinessService
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
            registerComponent.RegisterType<IUserService, UserService>();
            registerComponent.RegisterType<IAccountService, AccountService>();
            registerComponent.RegisterType<IRefreshTokenService, RefreshTokenService>();
            registerComponent.RegisterType<IPostsService, PostsService>();
        }
    }
}
