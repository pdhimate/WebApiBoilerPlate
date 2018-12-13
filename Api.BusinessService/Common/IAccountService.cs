using Api.BusinessEntities;
using Api.BusinessEntities.AccountController;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.BusinessService.Common
{
    /// <summary>
    /// Provides services for User Account management
    /// </summary>
    public interface IAccountService
    {
        /// <summary>
        /// Returns true if a user with specified email id is already registered.
        /// </summary>
        /// <param name="emailId"></param>
        /// <returns></returns>
        bool UserExists(UserExistsReq userExistsDto);

        /// <summary>
        /// Registers a user of an external provider like google, facebook.
        /// </summary>
        /// <param name="registerExternalUserDto"></param>
        /// <param name="userLoginInfo"></param>
        /// <returns></returns>
        bool RegisterExternalUser(RegisterExternalUserReq registerExternalUserDto, UserLoginInfo userLoginInfo);

        /// <summary>
        /// Gets the user profile
        /// </summary>
        /// <returns></returns>
        ProfileDto GetProfile(long userId);

        /// <summary>
        /// Updates the specified profile.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="profileToUpdate"></param>
        /// <returns></returns>
        ProfileRes UpdateProfile(long userId, ProfileDto profileToUpdate);
    }
}
