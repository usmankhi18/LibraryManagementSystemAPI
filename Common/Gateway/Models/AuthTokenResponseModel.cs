using Newtonsoft.Json;

namespace Common.Gateway.Models
{
    public class AuthTokenResponseModel
    {
        [JsonProperty(PropertyName = "access_token")]
        public string AccessToken { get; set; }

        [JsonProperty(PropertyName = "token_type")]
        public string TokenType { get; set; }

        [JsonProperty(PropertyName = "expires_in")]
        public string ExpiresIn { get; set; }

        [JsonProperty(PropertyName = "device_status")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "unique_identifier")]
        public string UniqueIdentifier { get; set; }

        [JsonProperty(PropertyName = "refresh_token")]
        public string RefreshToken { get; set; }
        public string responseCode { get; set; }
        public string responseDesc { get; set; }
    }
}
