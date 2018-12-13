using Api.Data.Models.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Data.Access.Repositories.Security
{
    public interface IClientRepo : IGenericRepository<Client, string>
    {
    }

    public class ClientRepo : EntityFrameworkRepository<Client, string>, IClientRepo
    {
        public ClientRepo(AppDatabaseContext context) : base(context)
        {
        }

    }
}
