using Amazon;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Common.Gateway.Intefaces;
using Common.Gateway.Models;
using Common.Gateway.Utils;
using Global.AppSettings;
using System.Net;

namespace Common.Gateway.Services
{
    public class CognitoService : IGatewayService
    {
        AmazonCognitoIdentityProviderClient _adminProvider = new AmazonCognitoIdentityProviderClient(AppSettingKeys.awsAccessKeyId, AppSettingKeys.awsAccessSecretId, RegionEndpoint.GetBySystemName(AppSettingKeys.region));

        public async Task<bool> DeleteAsync(string accessToken)
        {
            try
            {
                var globalSignOutResponse = await _adminProvider.GlobalSignOutAsync(new GlobalSignOutRequest() { AccessToken = accessToken });
                return globalSignOutResponse?.HttpStatusCode.ToString() == HttpStatusCode.OK.ToString();
            }
            catch (NotAuthorizedException)
            {
                return false;
            }
        }

        public async Task<AuthTokenResponseModel> GenerateToken(string userName, string password, string grantType, string basicToken, string clientId = null, string clientSecret = null)
        {
            AuthTokenResponseModel authTokenResponseModel = new AuthTokenResponseModel();
            try
            {                
                try
                {
                    // delete previous sessions (stop multi-login)
                    var globalSignOutResponse = await _adminProvider.AdminUserGlobalSignOutAsync(new AdminUserGlobalSignOutRequest()
                    {
                        Username = userName,
                        UserPoolId = AppSettingKeys.user_pool_id
                    });
                    Thread.Sleep(1000);
                }
                catch (Exception ex)
                {

                }

            

                //UserNotFoundException



                var authReq = new InitiateAuthRequest
                {
                    //ClientId = AppSettingKeys.client_id, //credentials[0],
                    ClientId = basicToken,
                    AuthFlow = AuthFlowType.USER_PASSWORD_AUTH

                };
                authReq.AuthParameters.Add("USERNAME", userName);
                authReq.AuthParameters.Add("PASSWORD", password);

                InitiateAuthResponse authResponse = await _adminProvider.InitiateAuthAsync(authReq);
                // if challenge name is not empty and value = 'NEW_PASSWORD_REQUIRED' then it is b2b user and password needs to be updated
                if (authResponse.ChallengeName == "NEW_PASSWORD_REQUIRED")
                {
                    //b2b users popup flow
                    authTokenResponseModel.responseCode = "001";
                    authTokenResponseModel.responseDesc = "Reset password required, show popup and then call admin set up password api to set user's new password for permanent";

                    return authTokenResponseModel;
                }
                else
                {
                    //b2c users
                    authTokenResponseModel.AccessToken = authResponse.AuthenticationResult.AccessToken;
                    authTokenResponseModel.ExpiresIn = authResponse.AuthenticationResult.ExpiresIn.ToString();
                    authTokenResponseModel.TokenType = authResponse.AuthenticationResult.TokenType;

                    return authTokenResponseModel;
                }

            }
            catch (Exception ex) when (ex.Message.Contains("Incorrect username or password."))
            {
                authTokenResponseModel.responseCode = "002";
                authTokenResponseModel.responseDesc = "Incorrect username or password";
                return authTokenResponseModel;
            }
            catch (Exception ex) when (ex.Message.Contains("User is disabled."))
            {
                authTokenResponseModel.responseCode = "002";
                authTokenResponseModel.responseDesc = "User is disabled.";
                return authTokenResponseModel;
            }
            catch (Exception ex)
            {
                authTokenResponseModel.responseCode = "002";
                authTokenResponseModel.responseDesc = "Something went wrong.";
                return authTokenResponseModel;
            }
        }

        public async Task<bool> ChangePassword(string oldPassword, string newPassword, string accessToken)
        {
            try
            {
                var changePasswordResponse = await _adminProvider.ChangePasswordAsync(new ChangePasswordRequest()
                {
                    AccessToken = accessToken,
                    PreviousPassword = oldPassword,
                    ProposedPassword = newPassword
                });
                return changePasswordResponse?.HttpStatusCode.ToString() == HttpStatusCode.OK.ToString();
            }
            catch (NotAuthorizedException)
            {
                return false;
            }
        }

        public async Task<bool> ForgotPassword(string Password, string UserId)
        {
            try
            {
                // change status to confirmed for newly created user so that first time reset password is skipped.
                var adminSetUserPasswordResponse = await _adminProvider.AdminSetUserPasswordAsync(new AdminSetUserPasswordRequest()
                {
                    Password = Password,
                    Permanent = true,
                    UserPoolId = AppSettingKeys.user_pool_id,
                    Username = UserId
                });

                return adminSetUserPasswordResponse?.HttpStatusCode.ToString() == HttpStatusCode.OK.ToString();
            }
            catch (NotAuthorizedException) {
                return false;
            }
        }

        public string ResolveHeader(HttpRequestMessage Request, enmClaims claimName)
        {
            var headersDictionary = new Dictionary<string, string>();
            try
            {
                IEnumerable<string> result;

                if (claimName == enmClaims.UserID)
                {
                    return (Request.Headers.TryGetValues("X-Authenticated-UserId", out result)) ? result.FirstOrDefault() : null;
                }
                else if (claimName == enmClaims.ChannelID)
                {
                    return (Request.Headers.TryGetValues("X-Consumer-Custom-Id", out result)) ? result.FirstOrDefault() : "0";
                }
                else if (claimName == enmClaims.ConsumerName)
                {
                    return (Request.Headers.TryGetValues("X-Consumer-Username", out result)) ? result.FirstOrDefault() : null;
                }

                return null;
            }
            catch (Exception ex)
            {
                //FileLogger.Info(ex.Message + " " + ex.StackTrace);
                return null;
            }
        }

        public bool ValidateToken(string token)
        {
            return true;
        }
    }
}
