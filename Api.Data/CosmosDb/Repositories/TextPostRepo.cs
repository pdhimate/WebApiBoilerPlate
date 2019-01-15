using Api.Data.CosmosDb.Models;
using Api.Data.Migrations;
using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Data.CosmosDb.Repositories
{
    public interface ITextPostRepo : IGenericRepositoryAsync<TextPost, string>
    {
    }

    public class TextPostRepo : CosmosRepoBase<TextPost>, ITextPostRepo
    {
        public override string CollectionName => Cosmos_CreatePostsCollection.CollectionName;

        public TextPostRepo(DocumentClient documentClient) : base(documentClient)
        {
        }

    }
}
