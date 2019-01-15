using Api.Data.Access;
using Api.Data.CosmosDb.Helpers;
using Api.Data.CosmosDb.Models;
using Api.Data.Helpers;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Api.Data.CosmosDb.Repositories
{
    public abstract class CosmosRepoBase<TEntity> : IGenericRepositoryAsync<TEntity, string> where TEntity : CosmosModelBase
    {
        protected readonly DocumentClient Client;

        #region Abstract properties

        /// <summary>
        /// The CollectionId/name.
        /// This must be unique in the database.
        /// </summary>
        public abstract string CollectionName { get; }

        #endregion

        public CosmosRepoBase(DocumentClient documentClient)
        {
            Client = documentClient;
        }

        #region Methods that can be overridden

        protected virtual async Task<DocumentCollection> GetCollectionAsync()
        {
            return await Client.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(Constants.CosmosDb.DatabaseId, CollectionName));
        }

        #endregion

        #region IGenericRepo implementation

        #region Get

        // See: https://stackoverflow.com/questions/35482455/read-azure-documentdb-document-that-might-not-exist?rq=1
        public async Task<TEntity> GetByIdAsync(string id)
        {
            var feedOptions = new FeedOptions() { MaxItemCount = 1 };
            var collection = await GetCollectionAsync();
            var query = Client.CreateDocumentQuery<TEntity>(collection.SelfLink, feedOptions)
                              .Where(x => x.Id == id)
                              .AsDocumentQuery();

            var page = await query.ExecuteNextAsync<TEntity>();
            return page.ToList().FirstOrDefault();
        }

        // See: https://stackoverflow.com/questions/35482455/read-azure-documentdb-document-that-might-not-exist?rq=1
        public async Task<TEntity> GetByIdAsync<TPartitionKey>(string id, TPartitionKey partitionKey)
        {
            var feedOptions = new FeedOptions() { MaxItemCount = 1, PartitionKey = new PartitionKey(partitionKey) };
            var collection = await GetCollectionAsync();
            var query = Client.CreateDocumentQuery<TEntity>(collection.SelfLink, feedOptions)
                              .Where(x => x.Id == id)
                              .AsDocumentQuery();

            var page = await query.ExecuteNextAsync<TEntity>();
            return page.ToList().FirstOrDefault();
        }

        public async Task<PageC<TEntity>> GetPageAsync(string continuationToken, List<Expression<Func<TEntity, bool>>> filters)
        {
            // Create query feed options
            var feedOptions = new FeedOptions
            {
                MaxItemCount = Constants.CosmosDb.PaginationPageSize,
                RequestContinuation = continuationToken // This will be not null, in case there was a previous request made
            };

            // TODO: fix the where see: https://stackoverflow.com/questions/51226966/logical-and-for-linq-to-cosmosdb-sql-api
            // Combine all filter by ANDing them
            Expression<Func<TEntity, bool>> combinedFilter = null;
            if (filters != null && filters.Count > 0)
            {
                combinedFilter = filters[0];
                foreach (var f in filters.Skip(1))
                {
                    combinedFilter = combinedFilter.And(f);
                }
            }

            // Construct query
            var collection = await GetCollectionAsync();
            var query = Client.CreateDocumentQuery<TEntity>(collection.SelfLink, feedOptions)
                .Where(combinedFilter ?? (a => true))
                .AsDocumentQuery();

            // Fetch the page and set continuation token if there is another page
            continuationToken = null;
            var page = await query.ExecuteNextAsync<TEntity>();
            var queryResult = page.ToList();
            if (query.HasMoreResults)
            {
                continuationToken = page.ResponseContinuation;
            }

            // Create paged response
            var res = new PageC<TEntity>(queryResult, continuationToken);
            return res;
        }

        #endregion

        #region Upsert

        public async Task<TEntity> InsertOrUpdateAsync(TEntity entity)
        {
            TEntity upsertedEntity;

            var upsertedDoc = await Client.UpsertDocumentAsync((await GetCollectionAsync()).SelfLink, entity);
            upsertedEntity = upsertedDoc.Cast<TEntity>();

            return upsertedEntity;
        }

        public async Task<TEntity> InsertAsync(TEntity entity)
        {
            var newDoc = await Client.CreateDocumentAsync((await GetCollectionAsync()).SelfLink, entity);
            return newDoc.Cast<TEntity>();
        }

        #endregion

        #region Delete

        public async Task<bool> DeleteAsync(string id)
        {
            // Check if it exists, otherwise delete throws
            var doc = await GetByIdAsync(id);
            if (doc == null)
            {
                return true;
            }

            // Delete
            var uri = UriFactory.CreateDocumentUri(Constants.CosmosDb.DatabaseId, CollectionName, id);
            var result = await Client.DeleteDocumentAsync(uri);
            return result.StatusCode == HttpStatusCode.NoContent;
        }

        public async Task<bool> DeleteAsync<TPartitionKey>(string id, TPartitionKey partitionKey)
        {
            // Check if it exists, otherwise delete throws
            var doc = await GetByIdAsync(id, partitionKey);
            if (doc == null)
            {
                return true;
            }

            // Delete
            var uri = UriFactory.CreateDocumentUri(Constants.CosmosDb.DatabaseId, CollectionName, id);
            var reqOptions = new RequestOptions { PartitionKey = new PartitionKey(partitionKey) };
            var result = await Client.DeleteDocumentAsync(uri, reqOptions);
            return result.StatusCode == HttpStatusCode.NoContent;
        }

        #endregion

        #region Count

        public async Task<long> CountAsync()
        {
            return Client.CreateDocumentQuery<TEntity>((await GetCollectionAsync()).SelfLink).LongCount();
        }

        #endregion

        #endregion
    }
}
