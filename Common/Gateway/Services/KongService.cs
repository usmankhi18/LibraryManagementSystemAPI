using System.Net;
using System.Text;
using System.Web;
using Common.Gateway.Intefaces;
using Common.Gateway.Models;
using Common.Gateway.Utils;
using Global.AppSettings;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Common.Gateway.Services
{
    public class KongService : IGatewayService
    {
        private HttpClient _accessTokenClient;
        private HttpClient _kongAdminClient;
        private readonly HttpClientHandler httpClientHandler;

        private string _adminUrl;
        private string _accessTokenUrl;
        private string _expiryTokenUrl;
        private string _consumerDetailsUrl;
        private string _oAuthDetailsUrl;

        public KongService()
        {
            _accessTokenUrl = AppSettingKeys.KongAccessTokenURl;
            _adminUrl = AppSettingKeys.KongAdminUrl;
            _expiryTokenUrl = AppSettingKeys.TokenExpiryEndpoint;
            _consumerDetailsUrl = AppSettingKeys.ConsumersEndpoint;
            _oAuthDetailsUrl = AppSettingKeys.OAuthDetailsEndpoint;


            httpClientHandler = new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; }
            };

            _accessTokenClient = new HttpClient(httpClientHandler)
            {
                BaseAddress = new Uri(_accessTokenUrl)
            };

            _kongAdminClient = new HttpClient(httpClientHandler)
            {
                BaseAddress = new Uri(_adminUrl)
            };

            ServicePointManager.Expect100Continue = true;
            System.Net.ServicePointManager.ServerCertificateValidationCallback += delegate (object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                    System.Security.Cryptography.X509Certificates.X509Chain chain,
                    System.Net.Security.SslPolicyErrors sslPolicyErrors)
            {
                return true; // **** Always accept
            };
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11
                                                 | SecurityProtocolType.Tls12
                                                 | SecurityProtocolType.Tls;

        }

        public async Task<AuthTokenResponseModel> GenerateToken(string userName, string password, string grantType, string basicToken, string clientId = null, string clientSecret = null)
        {
            AuthTokenResponseModel token = new AuthTokenResponseModel();
            if (grantType.ToLower() == "password")
            {
                string requestBody = string.Empty;

                requestBody = $@"{{ 
                                ""provision_key"":""function"",
                                ""authenticated_userid"":""{userName}"",
                                ""username"":""{userName}"",
                                ""password"":""{password}"",
                                ""grant_type"":""{grantType}""
                             }}";

                _accessTokenClient.DefaultRequestHeaders.Add("Authorization", basicToken);

                Tuple<string, bool> response = await PostAsync(_accessTokenUrl, requestBody);

                if (response.Item2)
                {
                    dynamic responseModel = JObject.Parse(response.Item1);

                    token = new AuthTokenResponseModel()
                    {
                        AccessToken = responseModel.access_token,
                        TokenType = responseModel.token_type,
                        ExpiresIn = responseModel.expires_in,
                        RefreshToken = responseModel.refresh_token,
                    };
                }
            }
            else // Client Credentials
            {
                string responseContent = AccessClientCredentialsToken("", grantType, "", "", clientId, clientSecret);
                if (!string.IsNullOrEmpty(responseContent))
                {
                    dynamic objDeserializedResponse = JsonConvert.DeserializeObject<dynamic>(responseContent);
                    token = new AuthTokenResponseModel()
                    {
                        AccessToken = objDeserializedResponse.access_token,
                        TokenType = objDeserializedResponse.token_type,
                        ExpiresIn = objDeserializedResponse.expires_in
                    };
                }
            }
            return token;
        }

        public string AccessClientCredentialsToken(string BasicToken, string GrantType, string Username, string Password, string ClientID = null, string ClientSecret = null)
        {
            try
            {
                string postData = "";
                string url = _accessTokenUrl;
                Dictionary<string, string> data = new Dictionary<string, string>();
                if (ClientID != null && ClientSecret != null)
                {
                    data.Add("client_id", ClientID);
                    data.Add("client_secret", ClientSecret);
                }
                else
                {

                    data.Add("provision_key", "function");
                    data.Add("authenticated_userid", Username);
                    data.Add("username", Username);
                    data.Add("password", Password);
                }
                data.Add("grant_type", GrantType);
                foreach (string key in data.Keys)
                {
                    postData += HttpUtility.UrlEncode(key) + "="
                          + HttpUtility.UrlEncode(data[key]) + "&";
                }

                HttpWebRequest myHttpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);
                ServicePointManager.Expect100Continue = true;
                System.Net.ServicePointManager.ServerCertificateValidationCallback += delegate (object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                        System.Security.Cryptography.X509Certificates.X509Chain chain,
                        System.Net.Security.SslPolicyErrors sslPolicyErrors)
                {
                    return true; // **** Always accept
                };
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3
                            | SecurityProtocolType.Tls11
                            | SecurityProtocolType.Tls12
                            | SecurityProtocolType.Tls;
                myHttpWebRequest.Method = "POST";

                byte[] PostData = Encoding.ASCII.GetBytes(postData);

                myHttpWebRequest.ContentType = "application/x-www-form-urlencoded";
                myHttpWebRequest.ContentLength = PostData.Length;
                if (ClientID == null && ClientSecret == null)
                    myHttpWebRequest.Headers.Add("Authorization", BasicToken);


                using (var stream = myHttpWebRequest.GetRequestStream())
                {
                    stream.Write(PostData, 0, PostData.Length);
                }

                var webResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                return new StreamReader(webResponse.GetResponseStream()).ReadToEnd();
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public async Task<AuthTokenResponseModel> GenerateRefreshToken(string refreshToken, string clientId, string clientSecret, string basicToken)
        {
            string requestBody = string.Empty;
            AuthTokenResponseModel token = new AuthTokenResponseModel();

            requestBody = $@"{{ 
                                ""refresh_token"":""{refreshToken}"",
                                ""client_id"":""{clientId}"",
                                ""client_secret"":""{clientSecret}"",
                                ""grant_type"":""refresh_token""
                             }}";

            _accessTokenClient.DefaultRequestHeaders.Add("Authorization", basicToken);

            Tuple<string, bool> response = await PostAsync(_accessTokenUrl, requestBody);

            if (response.Item2)
            {
                dynamic responseModel = JObject.Parse(response.Item1);

                token = new AuthTokenResponseModel()
                {
                    AccessToken = responseModel.access_token,
                    TokenType = responseModel.token_type,
                    ExpiresIn = responseModel.expires_in,
                    RefreshToken = responseModel.refresh_token,
                };
            }
            return token;
        }

        public async Task<Tuple<string, string>> GetClientDetail(string basicToken)
        {
            var kongResponse = new Tuple<string, bool>(string.Empty, false);
            var response = new Tuple<string, string>(string.Empty, string.Empty);
            try
            {
                kongResponse = await this.GetOAuthDetails(basicToken);
                if (kongResponse.Item2)
                {
                    var oAuth2Details = JsonConvert.DeserializeObject<OAuth2Details>(kongResponse.Item1);
                    if (oAuth2Details.data.Any())
                    {

                        response = new Tuple<string, string>(oAuth2Details.data.FirstOrDefault().client_id, oAuth2Details.data.FirstOrDefault().client_secret);

                    }
                    else
                    {

                        response = new Tuple<string, string>(string.Empty, string.Empty);

                    }
                }
                return response;
            }
            catch (Exception ex)
            {

            }

            return response;
        }

        public async Task<Tuple<string, bool>> GetConsumerDetails(string consumerId)
        {
            var httpResponse = await _kongAdminClient.GetAsync($"{_adminUrl}{_consumerDetailsUrl}{consumerId}");
            var response = new Tuple<string, bool>(await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false), httpResponse.IsSuccessStatusCode);
            return response;
        }


        public async Task<Tuple<string, bool>> GetOAuthDetails(string basicToken)
        {
            var base64String = System.Convert.FromBase64String(basicToken);
            string clientId = System.Text.Encoding.UTF8.GetString(base64String)?.Split(':')?.FirstOrDefault();

            var httpResponse = await _kongAdminClient.GetAsync($@"{_adminUrl}{_oAuthDetailsUrl}?client_id={clientId}");
            var response = new Tuple<string, bool>(await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false), httpResponse.IsSuccessStatusCode);

            return response;
        }

        public async Task<Tuple<string, bool>> GetCustomId(string basicToken)
        {
            var response = new Tuple<string, bool>(string.Empty, false);
            try
            {
                response = await this.GetOAuthDetails(basicToken);
                if (response.Item2)
                {
                    var oAuth2Details = JsonConvert.DeserializeObject<OAuth2Details>(response.Item1);
                    if (oAuth2Details.data.Any())
                    {
                        response = await this.GetConsumerDetails(oAuth2Details.data.FirstOrDefault().consumer_id);

                        if (response.Item2)
                        {
                            dynamic responseModel = JObject.Parse(response.Item1);
                            response = new Tuple<string, bool>(Convert.ToString(responseModel.custom_id), response.Item2);
                        }
                    }
                    else
                    {
                        response = new Tuple<string, bool>(string.Empty, false);
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return response;
        }

        public async Task<Tuple<int, bool>> GetUserActiveSessions(string authenticatedUserId)
        {
            var response = new Tuple<int, bool>(0, false);
            try
            {
                var httpResponse = await _kongAdminClient.GetAsync($"{_adminUrl}{_expiryTokenUrl}?authenticated_userid={authenticatedUserId}").ConfigureAwait(false);
                var activeSessionsResponse = new Tuple<string, bool>(await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false), httpResponse.IsSuccessStatusCode);

                if (activeSessionsResponse.Item2)
                {
                    dynamic responseModel = JObject.Parse(activeSessionsResponse.Item1);
                    response = new Tuple<int, bool>(Convert.ToInt32(responseModel.total), activeSessionsResponse.Item2);
                }
            }
            catch (Exception ex)
            {
                //FileLogger.Info("Exception : " + ex.ToString() + " Stack Trace : " + ex.StackTrace);
            }
            return response;
        }

        public virtual async Task<Tuple<string, bool>> PostAsync(string resourceUrl, string body)
        {
            var content = new StringContent(body, Encoding.UTF8, "application/json");
            var httpResponse = await _accessTokenClient.PostAsync(resourceUrl, content).ConfigureAwait(false);
            var response = new Tuple<string, bool>(await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false), httpResponse.IsSuccessStatusCode);
            return response;
        }

        public async Task<bool> DeleteActiveSessions(string authenticatedUserId)
        {
            bool response = false;

            var httpResponse = await _kongAdminClient.GetAsync($"{_adminUrl}{_expiryTokenUrl}?authenticated_userid={authenticatedUserId}").ConfigureAwait(false);
            var activeSessionsResponse = new Tuple<string, bool>(await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false), httpResponse.IsSuccessStatusCode);

            if (activeSessionsResponse.Item2)
            {
                dynamic responseModel = JObject.Parse(activeSessionsResponse.Item1);
                var activeSessions = responseModel.data;

                if (Convert.ToInt32(responseModel.total) == 0)
                {
                    response = true;
                }
                else
                {
                    foreach (var session in activeSessions)
                    {
                        response = await DeleteAsync(Convert.ToString(session.access_token));
                        if (!response)
                        {
                            break;
                        }
                    }
                }
            }
            return response;
        }

        public async Task<bool> DeleteAsync(string accessToken)
        {
            bool response = false;

            if (!string.IsNullOrEmpty(accessToken))
            {
                var responseMessage = await _kongAdminClient.DeleteAsync($"{_adminUrl}{_expiryTokenUrl}{accessToken}").ConfigureAwait(false);
                if (responseMessage.IsSuccessStatusCode)
                {
                    response = true;
                }
            }
            return response;
        }

        public bool ValidateToken(string token)
        {
            return true;
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

        public async Task<bool> ChangePassword(string oldPassword, string newPassword, string accessToken)
        {
            return true;
        }
    }
    public class OAuth2Details
    {
        public string total { get; set; }
        public List<KongOAuth> data { get; set; }
    }
    public class KongOAuth
    {
        public long created_at { get; set; }
        public string client_id { get; set; }
        public string id { get; set; }
        public List<string> redirect_uri { get; set; }
        public string name { get; set; }
        public string client_secret { get; set; }
        public string consumer_id { get; set; }
    }
}
