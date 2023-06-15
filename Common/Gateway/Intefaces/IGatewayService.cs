using Common.Gateway.Models;
using Common.Gateway.Utils;

namespace Common.Gateway.Intefaces
{
    public interface IGatewayService
    {
        Task<AuthTokenResponseModel> GenerateToken(string userName, string password, string grantType, string basicToken, string clientId = null, string clientSecret = null);
        bool ValidateToken(string token);
        string ResolveHeader(HttpRequestMessage Request, enmClaims claimName);
        Task<bool> DeleteAsync(string accessToken);
        Task<bool> ChangePassword(string oldPassword, string newPassword, string accessToken);

    }
}
