using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Common.Gateway.Intefaces;
using Common.Gateway.Models;
using Common.Gateway.Utils;
using Global.AppSettings;
using Microsoft.IdentityModel.Tokens;

namespace Common.Gateway.Services
{
    public class JWTService : IGatewayService
    {
        private static string Secret = "XCAP05H6LoKvbRRa/QkqLNMI7cOHguaRyHzyg7n5qEkGjQmtBhz4SzYh4Fqwjyi3KJHlSXKPwVu2+bXr6CtpgQ==";

        // These values should ideally be in database for different clients but for now we are supporting single client.
        private static string _JWTConsumerID = AppSettingKeys.JWTConsumerID;
        private static string _JWTConsumerName = AppSettingKeys.JWTConsumerName;
        private static string _JWTTokenExpiryMins = AppSettingKeys.JWTTokenExpiryMins;
        private static string _JWTClientID = AppSettingKeys.JWTClientID;
        private static string _JWTClientSecret = AppSettingKeys.JWTClientSecret;

        /// <summary>
        /// Generates Token
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="grantType"></param>
        /// <param name="basicToken"></param>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        /// <returns></returns>
        public async Task<AuthTokenResponseModel> GenerateToken(string userName, string password, string grantType, string basicToken, string clientId = null, string clientSecret = null)
        {
            try
            {
                if (clientId == null || clientSecret == null)
                {
                    var base64String = System.Convert.FromBase64String(basicToken.Split(' ')?.LastOrDefault());
                    clientId = System.Text.Encoding.UTF8.GetString(base64String)?.Split(':')?.FirstOrDefault();
                    clientSecret = System.Text.Encoding.UTF8.GetString(base64String)?.Split(':')?.LastOrDefault();
                }
                if (clientId != _JWTClientID || clientSecret != _JWTClientSecret)
                {
                    //FileLogger.Info("Basic JWT Token is not Valid.");
                    return new AuthTokenResponseModel();
                }

                AuthTokenResponseModel tokenResponseModel = new AuthTokenResponseModel();
                int expiryMins = Convert.ToInt32(_JWTTokenExpiryMins); // Minutes
                int expiresInSec = expiryMins * 60; // Seconds
                byte[] key = Convert.FromBase64String(Secret);
                SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);
                SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[] {
                      new Claim("X-Authenticated-UserId", userName),
                      new Claim("X-Consumer-Custom-Id", _JWTConsumerID),
                      new Claim("X-Consumer-Username", _JWTConsumerName)
                }),
                    Issuer = "yourdomainname",
                    Expires = DateTime.UtcNow.AddMinutes(expiryMins),
                    SigningCredentials = new SigningCredentials(securityKey,
                    SecurityAlgorithms.HmacSha256Signature)
                };

                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                JwtSecurityToken token = handler.CreateJwtSecurityToken(descriptor);
                tokenResponseModel = new AuthTokenResponseModel()
                {
                    AccessToken = handler.WriteToken(token),
                    TokenType = "bearer",
                    ExpiresIn = Convert.ToString(expiresInSec),
                    RefreshToken = "",
                };
                return tokenResponseModel;
            }
            catch (Exception ex)
            {
                //FileLogger.Info(ex.Message + " " + ex.StackTrace);
                return new AuthTokenResponseModel();
            }
        }


        /// <summary>
        /// Validates Token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public bool ValidateToken(string token)
        {
            try
            {
                ClaimsPrincipal principal = GetPrincipal(token);
                if (principal == null)
                    return false;
                else
                    return true;
            }
            catch (Exception ex)
            {
                //FileLogger.Info(ex.Message + " " + ex.StackTrace);
                return false;
            }
        }

        /// <summary>
        /// Get Principle
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static ClaimsPrincipal GetPrincipal(string token)
        {
            try
            {
                var token1 = new JwtSecurityToken(jwtEncodedString: token);

                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwtToken = (JwtSecurityToken)tokenHandler.ReadToken(token);
                if (jwtToken == null)
                    return null;
                byte[] key = Convert.FromBase64String(Secret);
                TokenValidationParameters parameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
                SecurityToken securityToken;
                ClaimsPrincipal principal = tokenHandler.ValidateToken(token,
                      parameters, out securityToken);
                return principal;
            }
            catch (Exception ex)
            {
                //FileLogger.Info(ex.Message + " " + ex.StackTrace);
                return null;
            }
        }

        /// <summary>
        /// Resolves Header
        /// </summary>
        /// <param name="Request"></param>
        /// <returns></returns>
        public string ResolveHeader(HttpRequestMessage Request, enmClaims claimName)
        {
            var headersDictionary = new Dictionary<string, string>();
            bool isValid = true;
            try
            {
                var token = Request.Headers.Authorization.Parameter;
                if (token == null)
                {
                    isValid = false;
                }
                ClaimsPrincipal principal = GetPrincipal(token);
                if (principal == null)
                    isValid = false;
                ClaimsIdentity identity = null;
                try
                {
                    identity = (ClaimsIdentity)principal.Identity;
                }
                catch (NullReferenceException)
                {
                    isValid = false;
                }

                if (claimName == enmClaims.UserID)
                {
                    return (isValid && !string.IsNullOrEmpty(identity.FindFirst("X-Authenticated-UserId").Value)) ? identity.FindFirst("X-Authenticated-UserId").Value : null;
                }
                else if (claimName == enmClaims.ChannelID)
                {
                    return (isValid && !string.IsNullOrEmpty(identity.FindFirst("X-Consumer-Custom-Id").Value)) ? identity.FindFirst("X-Consumer-Custom-Id").Value : "0";
                }
                else if (claimName == enmClaims.ConsumerName)
                {
                    return (isValid && !string.IsNullOrEmpty(identity.FindFirst("X-Consumer-Username").Value)) ? identity.FindFirst("X-Consumer-Username").Value : null;
                }

                return null;
            }
            catch (Exception ex)
            {
                //FileLogger.Info(ex.Message + " " + ex.StackTrace);
                return null;
            }
        }

        public async Task<bool> DeleteAsync(string accessToken)
        {
            return true;
        }

        public async Task<bool> ChangePassword(string oldPassword, string newPassword, string accessToken)
        {
            return true;
        }
    }
}
