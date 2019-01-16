using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Data.CosmosDb.Helpers
{
    public class CosmosDbHelper
    {
        /// <summary>
        /// Creates and return a new instance of DocumentClient.
        /// Note: Always dispose such a client after use.
        /// </summary>
        /// <returns></returns>
        public static DocumentClient GetDocumentClient()
        {
            var url = ConfigurationManager.AppSettings[Constants.CosmosDb.CosmosDbUriKey];
            var authKey = ConfigurationManager.AppSettings[Constants.CosmosDb.CosmosDbPrimaryKey];
            var client = new DocumentClient(new Uri(url), authKey);
            return client;
        }

    }
}
