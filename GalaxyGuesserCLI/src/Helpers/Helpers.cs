using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace GalaxyGuesserCLI.Helpers
{
    public static class Helper
    {
        public static string GetStoredToken()
        {
            try
            {
                if (File.Exists("token.json"))
                {
                    var tokenJson = File.ReadAllText("token.json");
                    var tokenData = JsonSerializer.Deserialize<GoogleTokenResponse>(tokenJson);
                    return tokenData?.AccessToken;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading token: {ex.Message}");
            }
            return null;
        }
        public static string ExtractGuidFromToken(string jwtToken)
        {
            if (string.IsNullOrWhiteSpace(jwtToken))
                throw new ArgumentException("JWT token cannot be empty");

            try 
            {
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(jwtToken);
                
                return token.Claims.FirstOrDefault(c => c.Type == "sub")?.Value 
                    ?? token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            }
            catch (Exception ex) when (ex is ArgumentException || ex is FormatException)
            {
                throw new InvalidOperationException("Invalid JWT token format", ex);
            }
        }
    }

    public class GoogleTokenResponse
        {
            [JsonPropertyName("access_token")]
            public string AccessToken { get; set; } = string.Empty;
            
            [JsonPropertyName("id_token")]
            public string IdToken { get; set; } =  string.Empty;
            
            [JsonPropertyName("refresh_token")]
            public string RefreshToken { get; set; }  = string.Empty;
            
            [JsonPropertyName("expires_in")]
            public int ExpiresIn { get; set; }
        }

        
}
