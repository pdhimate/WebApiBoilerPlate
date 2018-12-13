using Api.BusinessEntities;
using Api.BusinessEntities.AccountController;
using Api.BusinessService;
using Api.Data.Access;
using Api.Data.Models.Security;
using AutoMapper;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System;

namespace Api.BusinessService.Common
{
    public class AccountService : BaseService, IAccountService
    {
        public AccountService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        #region IAccountService implementation

        public ProfileDto GetProfile(long userId)
        {
            var user = GetById(userId, UnitOfWork.AppUsers);
            var dto = Mapper.Map<ProfileDto>(user);
            dto.Roles = GetUserRoles(user);
            return dto;
        }

        public ProfileRes UpdateProfile(long userId, ProfileDto profileToUpdate)
        {
            if (profileToUpdate == null)
            {
                throw new BusinnessException($"{nameof(ProfileDto)} is null.");
            }

            var user = GetById(userId, UnitOfWork.AppUsers);
            user.FirstName = profileToUpdate.FirstName;
            user.MiddleName = profileToUpdate.MiddleName;
            user.LastName = profileToUpdate.LastName;
            user.PhoneNumber = profileToUpdate.PhoneNumber;

            UnitOfWork.AppUsers.Update(user);
            SaveChanges();

            return new ProfileRes();
        }

        public bool UserExists(UserExistsReq userExistsDto)
        {
            var user = UnitOfWork.AppUsers.GetByEmail(userExistsDto.EmailId);
            if (user == null)
            {
                return false;
            }

            return true;
        }

        public bool RegisterExternalUser(RegisterExternalUserReq registerExternalUserDto, UserLoginInfo userLoginInfo)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Local helpers

        public virtual List<string> GetUserRoles(AppUser appUser)
        {
            var rolesMap = UnitOfWork.AppUserRoleMap.GetByUserId(appUser.Id);
            var roles = new List<string>();
            foreach (var roleMap in rolesMap)
            {
                var role = UnitOfWork.AppRoles.GetById(roleMap.RoleId);
                roles.Add(role.Name);
            }

            return roles;
        }


        #endregion
    }
}
