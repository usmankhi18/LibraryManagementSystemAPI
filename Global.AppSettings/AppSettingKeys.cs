namespace Global.AppSettings
{
    public class AppSettingKeys
    {
        // Getter and setter for "MongoDBDatabase" key
        public static string MongoDBDatabase { get; set; }

        // Getter and setter for "MongoDBCollection" key
        public static string MongoDBCollection { get; set; }

        // Getter and setter for "RedisConnectionString" key
        public static string RedisConnectionString { get; set; }

        // Getter and setter for "RedisKey" key
        public static string RedisKey { get; set; }

        // Getter and setter for "MongoDBConnection" key
        public static string MongoDBConnection { get; set; }

        // Getter and setter for "OracleDBConnection" key
        public static string OracleDBConnection { get; set; }

        // Getter and setter for "PostgreSQLConnection" key
        public static string PostgreSQLConnection { get; set; }

        // Getter and setter for "SQLServerConnection" key
        public static string SQLServerConnection { get; set; }

        // Getter and setter for "EncryptionKey" key
        public static string EncryptionKey { get; set; }

        // Getter and setter for "ConnType" key
        public static string ConnType { get; set; }

        public static string AuthName { get; set; }
        public static string Authorization { get; set; }
        public static string user_pool_id { get; set; }
        //public static string client_id { get; set; }
        public static string region { get; set; }
        public static string awsAccessKeyId { get; set; }
        public static string awsAccessSecretId { get; set; }
        public static string KongAccessTokenURl { get; set; }
        public static string KongAdminUrl { get; set; }
        public static string TokenExpiryEndpoint { get; set; }
        public static string ConsumersEndpoint { get; set; }
        public static string OAuthDetailsEndpoint { get; set; }
        public static string JWTConsumerID { get; set; }
        public static string JWTConsumerName { get; set; }
        public static string JWTTokenExpiryMins { get; set; }
        public static string JWTClientID { get; set; }
        public static string JWTClientSecret { get; set; }
    }
}