using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GalaxyGuesserApi.src.Middleware
{
    public class TokenValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _config;

        public TokenValidationMiddleware(RequestDelegate next, IConfiguration config)
        {
            _next = next;
            _config = config;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value?.ToLower();

            // Skip token validation for login and callback routes
            if (path != null && (path.Contains("/api/auth/login") || path.Contains("/api/auth/callback")))
            {
                await _next(context);
                return;
            }
            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(authHeader) && authHeader.StartsWith("Bearer "))
            {
                var idToken = authHeader["Bearer ".Length..];

                var clientId = _config["Google:ClientId"];
                var isValid = await TokenHelper.IsValidIdTokenAsync(clientId, idToken);

                if (!isValid)
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Invalid ID token.");
                    return;
                }

                var claims = TokenHelper.ParseIdToken(idToken);
                context.Items["Guid"] = claims["sub"];
                context.Items["Username"] = claims["given_name"];
            }
            else
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("401 - Authorization token missing.");
                return;
            }

            await _next(context);
        }
    }
}