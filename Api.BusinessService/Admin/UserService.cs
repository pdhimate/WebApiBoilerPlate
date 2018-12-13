using System.Collections.Generic;
using System.Linq;
using Api.Data.Access;
using AutoMapper;
using Api.BusinessEntities;
using System;
using Api.BusinessEntities.UserController;
using Api.BusinessEntities.Common;

namespace Api.BusinessService.Admin
{
    public interface IUserService
    {
        UserResDto GetById(long id);
        UserResDto GetUserByEmail(string email);
        PagedRes<UserResDto> GetUsers(int pageNumber, int pageSize);
        bool UpdateUser(long id, UserResDto userEntity);
        bool DeleteUser(long id);
    }

    public class UserService : BaseService, IUserService
    {
        public UserService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        #region IUserService implementation

        public UserResDto GetUserByEmail(string email)
        {
            var user = UnitOfWork.AppUsers.GetByEmail(email);
            if (user == null)
            {
                throw new BusinnessException($"User with email: {email} was not found.");
            }

            var userEntity = Mapper.Map<UserResDto>(user);
            return userEntity;
        }

        public bool DeleteUser(long id)
        {
            return DeleteById(id, UnitOfWork.AppUsers);
        }

        public PagedRes<UserResDto> GetUsers(int pageNumber, int pageSize)
        {
            var pagedResults = GetPage(pageNumber, pageSize, UnitOfWork.AppUsers, t => t.Id);
            var userEntities = Mapper.Map<PagedRes<UserResDto>>(pagedResults);
            return userEntities;
        }

        public UserResDto GetById(long id)
        {
            var user = GetById(id, UnitOfWork.AppUsers);
            var userDto = Mapper.Map<UserResDto>(user);
            return userDto;
        }

        public bool UpdateUser(long id, UserResDto userDto)
        {
            if (userDto == null)
            {
                throw new ArgumentNullException($"{nameof(userDto)} can not be null.");
            }

            var user = GetById(id, UnitOfWork.AppUsers);
            if (userDto.AddressId.HasValue)
            {
                user.DefaultAddressId = userDto.AddressId.Value;
            }
            user.Email = userDto.Email;
            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;
            user.MiddleName = userDto.MiddleName;
            user.PhoneNumber = userDto.PhoneNumber;

            UnitOfWork.AppUsers.Update(user);
            UnitOfWork.Save();

            return true;
        }

        #endregion

    }
}
