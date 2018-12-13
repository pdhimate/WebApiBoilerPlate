using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;

namespace Api.Data.Models.Security
{
    /// <summary>
    /// A Role is used to group users into groups (roles) and then set permissions on the role rather than for individual users (unlike Claims).
    /// E.g: we can have a role called Admin and provide operations to do "EditUser, DeleteUser, GetAllUsers" tasks.
    /// </summary>
    public class AppRole : IdentityRole<long, AppUserAppRoleMapping>
    {
    }

    /// <summary>
    /// The user roles that can be used within the application.
    /// </summary>
    public class AppRoles
    {
        public static List<string> GetAll()
        {
            return new List<string>
            {
                AdminRole
            };
        }

        public const string AdminRole = "Admin";
    }
}
