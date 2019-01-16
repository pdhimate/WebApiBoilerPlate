using Api.Auth;
using Api.BusinessEntities;
using Api.BusinessEntities.AccountController;
using Api.BusinessEntities.Common;
using Api.BusinessEntities.PostsController;
using Api.BusinessService.Common;
using Api.Data.Models.Security;
using Api.Resources;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace Api.Controllers
{
    /// <summary>
    /// Provides API for adding, fetching Posts.
    /// </summary>
    public class PostsController : BaseApiController
    {
        #region Private dependencies

        private readonly IPostsService _postsService;

        #endregion

        public PostsController(IPostsService postsService)
        {
            _postsService = postsService;
        }

        #region Api calls

        /// <summary>
        /// Creates a new TextPost 
        /// </summary>
        /// <param name="req"></param>
        /// <returns>The newly created text post</returns>
        [HttpPost]
        [ResponseType(typeof(ApiResponseDto<TextPostRes>))]
        public async Task<IHttpActionResult> TextPost(CreateTextPostReq req)
        {
            var userId = GetUserIdFromContext();
            return Ok(await _postsService.CreateTextPostAsync(userId, req));
        }

        /// <summary>
        /// Gets TextPosts in a paginated fashion
        /// </summary>
        /// <param name="req"></param>
        /// <returns>Page of posts</returns>
        [HttpPost]
        [ResponseType(typeof(ApiResponseDto<PagedResC<TextPostRes>>))]
        public async Task<IHttpActionResult> Page(PagedReqC req)
        {
            var userId = GetUserIdFromContext();
            return Ok(await _postsService.GetPageDesc(req, userId));
        }

        #endregion


    }
}