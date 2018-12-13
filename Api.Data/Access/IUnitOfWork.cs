using Api.Data.Access.Repositories;
using Api.Data.Access.Repositories.Security;
using Api.Data.Models;

namespace Api.Data.Access
{
    /// <summary>  
    /// Unit of Work class responsible for DB transactions.
    /// Ensures that entities stay in memory. And a bunch transactions are committed at once, thereby reducing database hits.
    /// </summary>  
    public interface IUnitOfWork
    {
        #region Repositories

        IAppRoleRepository AppRoles { get; }
        IAppUserAppRoleMappingRepo AppUserRoleMap { get; }

        IAddressRepository Addresses { get; }
        IRefreshTokenRepo RefreshTokens { get; }

        IUserRepository AppUsers { get; }

        IExternalUserLoginRepository ExternalUserLogins { get; }

        #endregion

        /// <summary>
        /// Commits the pending changes to the database.
        /// </summary>
        /// <returns>Number of objects changed in the database</returns>
        int Save();
    }
}
