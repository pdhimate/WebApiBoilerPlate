using Api.Data.Models;

namespace Api.Data.Access.Repositories.Security
{
    public interface IExternalUserLoginRepository : IGenericRepository<ExternalUserLogin, long>
    {

    }

    public class ExternalUserLoginRepository : EntityFrameworkRepository<ExternalUserLogin, long>, IExternalUserLoginRepository
    {
        public ExternalUserLoginRepository(AppDatabaseContext context) : base(context)
        {
        }
    }
}
