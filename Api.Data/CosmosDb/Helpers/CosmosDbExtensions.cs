using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Data.CosmosDb.Helpers
{
    public static class CosmosDbExtensions
    {
        /// <summary>
        /// Converts JSON Document to C# object.
        /// Ensure that the C# class has been appropriately decorates for JSON attributes.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="document"></param>
        /// <returns></returns>
        public static async Task<T> Cast<T>(this Document document)
        {
            using (var stream = new MemoryStream())
            {
                using (var reader = new StreamReader(stream))
                {
                    document.SaveTo(stream);
                    stream.Position = 0;
                    return JsonConvert.DeserializeObject<T>(await reader.ReadToEndAsync());
                }
            }
        }

        /// <summary>
        /// Converts JSON Document to C# object.
        /// Ensure that the C# class has been appropriately decorates for JSON attributes.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="document"></param>
        /// <returns></returns>
        public static T Cast<T>(this ResourceResponse<Document> document)
        {
            return JsonConvert.DeserializeObject<T>(document.Resource.ToString());
        }
    }
}
