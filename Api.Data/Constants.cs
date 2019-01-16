namespace Api.Data
{
    public class Constants
    {
        public class DatabaseSchemas
        {
            /// <summary>
            /// Represents the core data schema name in the database.
            /// The core data schema is used to store core functionality related data.
            /// </summary>
            public const string CoreDataSchemaName = "Core";

            /// <summary>
            /// Represents the meta data schema name in the database.
            /// The meta data schema is used to store supportive functionalities related data like logs, exceptions etc.
            /// </summary>
            public const string MetaDataSchemaName = "Meta";

            /// <summary>
            /// Represents the security data schema name in the database.
            /// The security data schema is used to store sensitive data like user accounts tables, etc.
            /// </summary>
            public const string SecurityDataSchemaName = "Security";

            /// <summary>
            /// Represents the master data schema in the database.
            /// This stores all static data like countries, states, etc.
            /// </summary>
            public const string MasterDataSchemaName = "Master";
        }

        public class WebConfig
        {
            public const string DatabaseConnectionKey = "AppDatabaseConnection";

        }

        public class DefaultUsers
        {
            public static string AdminUserName { get { return "YourAdminUserEmail@SomeEmailProvider.com"; } }
        }

        public class CosmosDb
        {
            /// <summary>
            /// Url of the Azure Cosmos db.
            /// Must be present in the web.config/AppSettings
            /// </summary>
            public const string CosmosDbUriKey = "CosmosDbUri";

            /// <summary>
            /// Primary key of the Azure Cosmos db.
            /// Must be present in the web.config/AppSettings
            /// </summary>
            public const string CosmosDbPrimaryKey = "CosmosDbPrimaryKey";

            /// <summary>
            /// The Id or the name of the Cosmos database
            /// </summary>
            public static string DatabaseId { get { return "YourDocumentStore"; } }

            public static string PostsCollectionName { get { return "PostsCollection"; } }

            /// <summary>
            /// The PageSize used during pagination for CosmosDb.
            /// </summary>
            public static int PaginationPageSize { get { return 20; } }
        }
    }
}
