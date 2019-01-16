using Api.BusinessEntities;
using Api.BusinessEntities.PostsController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.BusinessService.Common
{
    public interface IPostsService
    {
        Task<TextPostRes> CreateTextPostAsync(long createdByUserId, CreateTextPostReq req);

        Task<PagedResC<TextPostRes>> GetPageDesc(PagedReqC req, long userId);

    }
}
