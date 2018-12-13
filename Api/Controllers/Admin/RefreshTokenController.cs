using Api.BusinessEntities.Common;
using Api.BusinessEntities.RefreshTokenController;
using Api.BusinessService.Admin;
using Api.Data.Models.Security;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace Api.Controllers.Admin
{
    [Authorize(Roles = AppRoles.AdminRole)]
    public class RefreshTokenController : BaseApiController
    {
        private readonly IRefreshTokenService _service = null;

        public RefreshTokenController(IRefreshTokenService service)
        {
            _service = service;
        }

        /// <summary>
        /// Gets the refresh tokens on the specified page.
        /// </summary>
        /// <param name="pagedReqDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ResponseType(typeof(ApiResponseDto<PagedRes<RefreshTokenRes>>))]
        public IHttpActionResult Tokens(PagedReqDto pagedReqDto)
        {
            PagedRes<RefreshTokenRes> pagedTokens = _service.GetPage(pagedReqDto.PageNumber,
                                                        pagedReqDto.PageSize);
            if (pagedTokens == null || !pagedTokens.Items.Any())
            {
                return NotFound();
            }

            return Ok(pagedTokens);
        }

        /// <summary>
        /// Deletes the specified refresh token, 
        /// thus forcing the client to create a new access token using user name and password
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpDelete]
        [ResponseType(typeof(ApiResponseDto<bool>))]
        public IHttpActionResult Delete(DeleteRefreshTokenReq req)
        {
            var result = _service.DeleteRefreshToken(req.Id);
            if (!result)
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}