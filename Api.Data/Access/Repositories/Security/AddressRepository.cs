using Api.Data.Models.Security;

namespace Api.Data.Access.Repositories.Security
{
    public interface IAddressRepository : IGenericRepository<Address, long>
    {

    }

    public class AddressRepository : EntityFrameworkRepository<Address, long>, IAddressRepository
    {
        public AddressRepository(AppDatabaseContext context) : base(context)
        {
        }
    }
}
