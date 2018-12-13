using Api.Data.Models.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Data.Access.Repositories.Security
{
    public interface IAppUserAppRoleMappingRepo : IGenericRepository<AppUserAppRoleMapping, long>
    {
        List<AppUserAppRoleMapping> GetByUserId(long userId);
    }

    public class AppUserAppRoleMappingRepo : EntityFrameworkRepository<AppUserAppRoleMapping, long>, IAppUserAppRoleMappingRepo
    {
        public AppUserAppRoleMappingRepo(AppDatabaseContext context) : base(context)
        {
        }

        public List<AppUserAppRoleMapping> GetByUserId(long userId)
        {
            return DbSet.Where(m => m.UserId == userId).ToList();
        }
    }
}