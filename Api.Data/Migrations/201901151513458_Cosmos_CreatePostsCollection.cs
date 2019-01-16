namespace Api.Data.Migrations
{
    using Api.Data.CosmosDb.Helpers;
    using Microsoft.Azure.Documents;
    using Microsoft.Azure.Documents.Client;
    using System;
    using System.Data.Entity.Migrations;
    using System.Threading.Tasks;

    public partial class Cosmos_CreatePostsCollection : DbMigration
    {
        #region Properties

        /// <summary>
        /// This must be unique in the database.
        /// </summary>
        public const string CollectionName = "PostsCollection";

        /// <summary>
        /// A logical partition key, which would be appropriately used for physical partitioning by CosmosDb
        /// </summary>
        public const string PartitionKeyPath = "/CreatedByUserId";

        #endregion

        public override async void Up()
        {
            using (var client = CosmosDbHelper.GetDocumentClient())
            {
                await CreatePostsCollectionAsync(client);
            }
        }

        #region Local helpers

        /// <summary>
        /// Creates activities collection if it does not already exist 
        /// </summary>
        /// <returns></returns>
        private async Task CreatePostsCollectionAsync(DocumentClient client)
        {
            var collection = new DocumentCollection
            {
                Id = CollectionName,
            };

            // Set partition key
            collection.PartitionKey.Paths.Add(PartitionKeyPath);

            // Set indexing policies
            collection.IndexingPolicy.ExcludedPaths.Add(new ExcludedPath { Path = "/*" }); // exclude all properties by default

            collection.IndexingPolicy.IncludedPaths.Add(new IncludedPath { Path = $"/CreatedOnUtc/?" }); // TODO: not sure if we need this indexed
            collection.IndexingPolicy.IncludedPaths.Add(new IncludedPath { Path = $"/CreatedByUserId/?" });
            collection.IndexingPolicy.IncludedPaths.Add(new IncludedPath { Path = $"/CreatedByUser/?" });

            // Set throughput
            var options = new RequestOptions
            {
                OfferThroughput = 400, // Default value is 10000. Currently set to minimum value to keep costs low on Azure.
            };

            // Create
            await client.CreateDocumentCollectionIfNotExistsAsync(
               UriFactory.CreateDatabaseUri(Constants.CosmosDb.DatabaseId),
               collection,
               options);
        }

        #endregion
    }
}
