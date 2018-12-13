using Api.Data.Models.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Data.Access.Repositories.Security
{
    public interface IAppRoleRepository : IGenericRepository<AppRole, long>
    {
    }

    public class AppRoleRepository : EntityFrameworkRepository<AppRole, long>, IAppRoleRepository
    {
        public AppRoleRepository(AppDatabaseContext context) : base(context)
        {
        }
    }
}