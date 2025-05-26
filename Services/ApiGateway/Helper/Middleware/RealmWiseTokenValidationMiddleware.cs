using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace ApiGateway.Helper.Middleware
{
    public class RealmWiseTokenValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RealmWiseTokenValidationMiddleware> _logger;

        public RealmWiseTokenValidationMiddleware(RequestDelegate next, ILogger<RealmWiseTokenValidationMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();

            if (string.IsNullOrWhiteSpace(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Missing or invalid Authorization header.");
                return;
            }

            var token = authHeader.Substring("Bearer ".Length).Trim();

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);
            var issuer = jwt.Issuer;

            var discoveryUrl = $"{issuer}/.well-known/openid-configuration";
            var configManager = new ConfigurationManager<OpenIdConnectConfiguration>(
                discoveryUrl,
                new OpenIdConnectConfigurationRetriever()
            );

            var config = await configManager.GetConfigurationAsync(CancellationToken.None);

            var validationParams = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = issuer,
                ValidateAudience = false,
                ValidateLifetime = true,
                IssuerSigningKeys = config.SigningKeys
            };

            try
            {
                var principal = handler.ValidateToken(token, validationParams, out _);
                context.User = principal; // Assign validated principal to context
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Token validation failed.");
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Token validation failed.");
            }
        }
    }
}
