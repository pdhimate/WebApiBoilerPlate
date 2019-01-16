using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Api.BusinessEntities;
using Api.BusinessEntities.PostsController;
using Api.Data.Access;
using Api.Data.CosmosDb.Models;
using AutoMapper;

namespace Api.BusinessService.Common
{
    public class PostsService : BaseService, IPostsService
    {
        public PostsService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        #region IPostsService

        public async Task<TextPostRes> CreateTextPostAsync(long createdByUserId, CreateTextPostReq req)
        {
            var user = UnitOfWork.AppUsers.GetById(createdByUserId);
            if (user == null)
            {
                throw new BusinnessException("Invalid user id: " + createdByUserId);
            }

            var textPost = Mapper.Map<TextPost>(req); // map to database model
            textPost.CreatedByUserId = user.Id;
            textPost.CreatedByUserName = user.FirstName + " " + user.LastName;

            var res = await UnitOfWork.TextPostRepo.InsertAsync(textPost);
            return Mapper.Map<TextPostRes>(res); // map to DTO
        }

        public async Task<PagedResC<TextPostRes>> GetPageDesc(PagedReqC req, long userId)
        {
            var user = UnitOfWork.AppUsers.GetById(userId);
            if (user == null)
            {
                throw new BusinnessException("Invalid user id: " + userId);
            }

            // Create filters only if this is a first page request
            List<Expression<Func<TextPost, bool>>> filters = null;
            var isFirstPageReq = string.IsNullOrWhiteSpace(req.ContinuationToken); // a non-empty token indicates a next page request
            if (isFirstPageReq)
            {
                filters = new List<Expression<Func<TextPost, bool>>>();
                
                // Although incidentally userId is also partition key, this serves as a reference to implement filters with pagination
                // Note: Only paths included for indexing in the collection, can be added here as filters.
                filters.Add(p => p.CreatedByUserId == userId); // user can see only his posts 

                // you may add more filters as desired here
            }

            // Get page
            var page = await UnitOfWork.TextPostRepo.GetPageDescendingAsync<long>(req.ContinuationToken, userId, filters);

            // Map to specific activities
            var resItems = Mapper.Map<IEnumerable<TextPostRes>>(page.Items);
            return new PagedResC<TextPostRes>(resItems, page.ContinuationToken);
        }

        #endregion
    }
}
