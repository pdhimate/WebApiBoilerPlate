using Api.BusinessEntities;
using Api.BusinessEntities.Common;
using Api.BusinessEntities.UserController;
using Api.BusinessService;
using Api.BusinessService.Admin;
using Api.Data.Models;
using Api.Data.Models.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace Api.Controllers.Admin
{
    /// <summary>
    /// Provides API for managing the user accounts for the users with Admin role.
    /// [Admin]
    /// </summary>
    [Authorize(Roles = AppRoles.AdminRole)]
    public class UserController : BaseApiController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        #region Api calls

        /// <summary>
        /// Gets the users on the specified page.
        /// </summary>
        /// <param name="pagedReqDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ResponseType(typeof(ApiResponseDto<PagedRes<UserResDto>>))]
        public IHttpActionResult Users(PagedReqDto pagedReqDto)
        {
            PagedRes<UserResDto> pagedUsers = _userService.GetUsers(pagedReqDto.PageNumber, pagedReqDto.PageSize);
            if (pagedUsers == null || !pagedUsers.Items.Any())
            {
                return NotFound();
            }

            return Ok(pagedUsers);
        }


        /// <summary>
        /// Gets the user associated with the specified Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(ApiResponseDto<UserResDto>))]
        public IHttpActionResult Users(long id)
        {
            var user = _userService.GetById(id);
            return Ok(user);
        }

        #endregion
    }
}