using Api.Data.Models.Security;
using System.Linq;
using System;

namespace Api.Data.Access.Repositories.Security
{
    public interface IUserRepository : IGenericRepository<AppUser, long>
    {
        AppUser GetByEmail(string emailId);

        Page<AppUser> GetUsers(int pageNumber, int pageSize);
    }

    public class UserRepository : EntityFrameworkRepository<AppUser, long>, IUserRepository
    {
        public UserRepository(AppDatabaseContext context) : base(context)
        {
        }

        #region IUserRepository implementation

        public AppUser GetByEmail(string emailId)
        {
            return DbSet.FirstOrDefault(user => user.Email == emailId);
        }

        /// <summary>
        /// Note: Some custom logic to fetch the results has been added here, instead of, in the BusinessLayer.
        /// The idea is to make the repository more useful since it is already caching entities into the memory.
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public Page<AppUser> GetUsers(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    $"Invalid pageNumber: {pageNumber} and pageSize: {pageSize}. They must be greater than 0.");
            }

            var totalCount = DbSet.LongCount();
            double pages = totalCount / pageSize;
            var pageCount = Math.Ceiling(pages);
            var skip = (pageNumber - 1) * pageSize;
            var users = DbSet.OrderBy(u => u.Id).Skip(skip).Take(pageSize).ToList();

            var pagedResult = new Page<AppUser>(users, totalCount, pageNumber, pageSize);
            return pagedResult;
        }

        #endregion
    }
}
