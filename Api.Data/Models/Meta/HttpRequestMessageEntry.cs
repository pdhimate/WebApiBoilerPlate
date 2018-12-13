using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace Api.Data.Models.Meta
{
    [Table("HttpRequestMessageEntries", Schema = Constants.DatabaseSchemas.MetaDataSchemaName)]
    public class HttpRequestMessageEntry : BaseEntry
    {
        public string Method { get; set; }
        public string RequestOriginalUri { get; set; }
        public string IdnHost { get; set; }
        public string Headers { get; set; }
        public string UserName { get; set; }
        public string IpAddress { get; set; }
    }
}
