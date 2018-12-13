using Api.Data.Models;
using Api.Data.Models.Security;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace Api.Auth
{
    public class ApplicationUserStore : UserStore<AppUser, AppRole, long, ExternalUserLogin, AppUserAppRoleMapping, AppUserClaim>
    {
        public ApplicationUserStore(DbContext context) : base(context)
        {
        }
    }
}
