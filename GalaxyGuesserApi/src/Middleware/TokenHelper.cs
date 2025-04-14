using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace GalaxyGuesserApi.src.Middleware
{
    public static class TokenHelper
    {
        private const string GoogleJwksUrl = "https://www.googleapis.com/oauth2/v3/certs";

        public static async Task<bool> IsValidIdTokenAsync(string clientId, string idToken)
        {
            var httpClient = new HttpClient();
            var jwksResponse = await httpClient.GetStringAsync(GoogleJwksUrl);
            var jwks = new JsonWebKeySet(jwksResponse);

            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = "https://accounts.google.com",
                ValidateAudience = true,
                ValidAudience = clientId,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKeys = jwks.Keys
            };

            try
            {
                tokenHandler.ValidateToken(idToken, validationParameters, out _);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static Dictionary<string, object> ParseIdToken(string idToken)
        {
            var parts = idToken.Split('.');
            if (parts.Length != 3)
                throw new ArgumentException("Invalid ID token format.");

            var payload = Base64UrlDecode(parts[1]);
            var json = JsonDocument.Parse(payload);

            return new Dictionary<string, object>
            {
                { "sub", json.RootElement.GetProperty("sub").GetString() },
                { "given_name", json.RootElement.GetProperty("given_name").GetString() },
            };
        }

        public static string ExtractUserIdFromToken(string idToken)
        {
            var tokenPayload = ParseIdToken(idToken);
            return tokenPayload["sub"]?.ToString();
        }

        private static string Base64UrlDecode(string input)
        {
            input = input.Replace('-', '+').Replace('_', '/');
            switch (input.Length % 4)
            {
                case 2: input += "=="; break;
                case 3: input += "="; break;
            }

            var bytes = Convert.FromBase64String(input);
            return System.Text.Encoding.UTF8.GetString(bytes);
        }
    }
}