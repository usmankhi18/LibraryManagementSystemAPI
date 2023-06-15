using Common.Gateway.Intefaces;
using Global.AppSettings;

namespace Common.Gateway.Services
{
    public static class GatewayService
    {
        /// <summary>
        /// AuthHandlerName
        /// </summary>
        public static string GatewayName = AppSettingKeys.AuthName;

        /// <summary>
        /// GetAuthHandler
        /// </summary>
        /// <returns></returns>
        public static IGatewayService GetAuthHandler()
        {
            if (GatewayName == "JWT")
            {
                return new JWTService();
            }
            else if (GatewayName == "Cognito")
            {
                return new CognitoService();
            }
            else
            {
                return new KongService();
            }
        }
    }
}
