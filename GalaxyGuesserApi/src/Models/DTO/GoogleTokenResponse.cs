
using System.Text.Json.Serialization;
namespace GalaxyGuesserApi.Models.DTO
{
    public class GoogleTokenResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; } = string.Empty;

        [JsonPropertyName("id_token")]
        public string IdToken { get; set; } = string.Empty;

        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; } = string.Empty;

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }
    }

        public class TokenRequest
        {
            public string Code { get; set; } = string.Empty;
            public string RedirectUri { get; set; } = string.Empty;
        }


}
