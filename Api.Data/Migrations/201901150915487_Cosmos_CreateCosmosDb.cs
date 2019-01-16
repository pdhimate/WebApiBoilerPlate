namespace Api.Data.Migrations
{
    using Api.Data.CosmosDb.Helpers;
    using Microsoft.Azure.Documents;
    using System;
    using System.Data.Entity.Migrations;
    using System.Threading.Tasks;

    public partial class Cosmos_CreateCosmosDb : DbMigration
    {
        public override async void Up()
        {
            await CreateAsync();
        }

        /// <summary>
        /// Creates a new database if it does not already exist
        /// </summary>
        /// <returns></returns>
        private async Task CreateAsync()
        {
            using (var client = CosmosDbHelper.GetDocumentClient())
            {
                await client.CreateDatabaseIfNotExistsAsync(new Database
                {
                    Id = Constants.CosmosDb.DatabaseId
                });
            }
        }

    }
}
