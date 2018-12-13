using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.Data.Access;
using Api.BusinessEntities.Common;
using Api.Data.Models.Security;
using AutoMapper;
using Api.BusinessEntities.RefreshTokenController;

namespace Api.BusinessService.Admin
{
    public interface IRefreshTokenService
    {
        PagedRes<RefreshTokenRes> GetPage(int pageNumber, int pageSize);
        RefreshTokenRes GetById(string id);

        /// <summary>
        /// Deletes the specified token, if found.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>True if found and deleted, otherwise false.</returns>
        bool DeleteRefreshToken(string id);
    }

    public class RefreshTokenService : BaseService, IRefreshTokenService
    {
        public RefreshTokenService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        #region IRefreshTokenService implementation

        public PagedRes<RefreshTokenRes> GetPage(int pageNumber, int pageSize)
        {
            var pagedResults = GetPage(pageNumber, pageSize, UnitOfWork.RefreshTokens, t => t.Id);
            var entities = Mapper.Map<PagedRes<RefreshTokenRes>>(pagedResults);

            // Set unmapped properties
            for (int i = 0; i < pagedResults.Items.Count(); i++)
            {
                var refreshTokenRes = entities.Items.ElementAt(i);
                var refreshToken = pagedResults.Items.ElementAt(i);

                SetUnMappedProperties(refreshTokenRes, refreshToken);
            }

            return entities;
        }

        public RefreshTokenRes GetById(string id)
        {
            var refreshToken = GetById(id, UnitOfWork.RefreshTokens);
            var res = Mapper.Map<RefreshTokenRes>(refreshToken);
            SetUnMappedProperties(res, refreshToken);
            return res;
        }

        public bool DeleteRefreshToken(string id)
        {
            return DeleteById(id, UnitOfWork.RefreshTokens);
        }

        #endregion

        #region Local helpers

        /// <summary>
        /// Sets certain properties which could not be set by AutoMapper
        /// </summary>
        /// <param name="refreshTokenRes"></param>
        /// <param name="refreshToken"></param>
        public virtual void SetUnMappedProperties(RefreshTokenRes refreshTokenRes, RefreshToken refreshToken)
        {
            refreshTokenRes.ClientName = refreshToken.Client.Name;
            refreshTokenRes.UserName = refreshToken.User.UserName;
        }



        #endregion

    }
}
