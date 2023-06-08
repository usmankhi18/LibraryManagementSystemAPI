using Common.Security;
using Global.AppSettings;
using Microsoft.AspNetCore.Authorization;
using System;

namespace LibraryManagementSystemAPI.Extensions
{
    public static partial class ConfigKeys
    {
        public static void LoadKeys(this IServiceCollection services, IConfiguration configuration)
        {
            AppSettingKeys.EncryptionKey = configuration["EncryptionKey"];
            AppSettingKeys.ConnType = configuration["ConnType"];
            AppSettingKeys.MongoDBDatabase = AESCryptoProvider.DecryptUsingCustomKey(AppSettingKeys.EncryptionKey,configuration["MongoDBDatabase"],true);
            AppSettingKeys.MongoDBCollection = AESCryptoProvider.DecryptUsingCustomKey(AppSettingKeys.EncryptionKey, configuration["MongoDBCollection"], true);
            AppSettingKeys.RedisConnectionString = AESCryptoProvider.DecryptUsingCustomKey(AppSettingKeys.EncryptionKey, configuration["RedisConnectionString"], true);
            AppSettingKeys.RedisKey = AESCryptoProvider.DecryptUsingCustomKey(AppSettingKeys.EncryptionKey, configuration["RedisKey"], true);
            AppSettingKeys.MongoDBConnection = AESCryptoProvider.DecryptUsingCustomKey(AppSettingKeys.EncryptionKey, configuration["MongoDBConnection"], true);
            AppSettingKeys.OracleDBConnection = AESCryptoProvider.DecryptUsingCustomKey(AppSettingKeys.EncryptionKey, configuration["OracleDBConnection"], true);
            AppSettingKeys.PostgreSQLConnection = AESCryptoProvider.DecryptUsingCustomKey(AppSettingKeys.EncryptionKey, configuration["PostgreSQLConnection"], true);
            AppSettingKeys.SQLServerConnection = AESCryptoProvider.DecryptUsingCustomKey(AppSettingKeys.EncryptionKey, configuration["SQLServerConnection"], true);
        }
       
    }
}
