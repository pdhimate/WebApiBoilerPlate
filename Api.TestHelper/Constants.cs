using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.TestHelper
{
    public static class Constants
    {
        public static class DatabaseStaticEntities
        {
            /// <summary>
            /// The emailid of the admin use, which is inserted with the seed data during the creation of the database
            /// </summary>
            public static string AdminUserEmail { get { return "YourEmailId @ gmail . com"; } }
            public static string AdminUserName { get { return "Admin"; } }
            public static string DefaultPassword { get { return "abcd12#$"; } }
        }

        public static class TestCategories
        {
            public const string ApiIntegrationTests = "ApiIntegrationTests";
            public const string DatabaseIntegrationTests = "DatabaseIntegrationTests";
            public const string UnitTests = "UnitTests";
        }

        /// <summary>
        /// The namespaces to be used while using Json or Xml serializers.
        /// These correspond to the default namespaces for the objects in the respective projects/namespaces.
        /// </summary>
        public static class SerializationNamespaces
        {
            public static string BusinessEntities { get { return "http://schemas.datacontract.org/2004/07/Api.BusinessEntities"; } }
        }

        public static class Webconfig
        {
            public const string ApiBaseUrlKey = "ApiBaseUrl";
        }
    }
}
