using Api.Data.Models.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Data.Access.Repositories.Security
{
    public interface IRefreshTokenRepo : IGenericRepository<RefreshToken, string>
    {
        /// <summary>
        /// Since this repo has been directly used in the UI layer, the method allows to save chnages to the db.
        /// </summary>
        void SaveChanges();

    }

    public class RefreshTokenRepo : EntityFrameworkRepository<RefreshToken, string>, IRefreshTokenRepo
    {
        public RefreshTokenRepo(AppDatabaseContext context) : base(context)
        {
        }

        #region overridden methods

        public override void Insert(RefreshToken token)
        {
            // Ensure that only one refresh token exists per user and per client.
            var existingToken = Context.RefreshTokens.SingleOrDefault(r => r.UserId == token.UserId
                                                                  && r.ClientId == token.ClientId);

            if (existingToken != null)
            {
                Delete(existingToken);
            }

            base.Insert(token);
        }

        public void SaveChanges()
        {
            Context.SaveChanges();
        }

        #endregion
    }
}
