using ApiGateway.Helper.Client;

namespace ApiGateway.Helper.Middleware
{
    public class AuthorizationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        private readonly List<PathString> _whitelistedPaths;
        private readonly ValidateAuthorizeClient _validateAuthorizeClient;

        public AuthorizationMiddleware(RequestDelegate next, IConfiguration configuration, ValidateAuthorizeClient validateAuthorizeClient)
        {
            _next = next;
            _configuration = configuration;
            _whitelistedPaths = _configuration.GetSection("PublicRoutes").Get<List<string>>().Select(p => new PathString(p)).ToList();
            _validateAuthorizeClient = validateAuthorizeClient;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var requestPath = context.Request.Path;

            if (_whitelistedPaths.Any(p => requestPath.Equals(p, StringComparison.OrdinalIgnoreCase)))
            {
                await _next(context);
                return;
            }

            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (string.IsNullOrWhiteSpace(token))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Missing or invalid Authorization header.");
                return;
            }
            try
            {
                var authResponse = await _validateAuthorizeClient.ValidateAuthorizeAsync(token);
                if (!authResponse.IsSuccess)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Invalid token.");
                    return;
                }
                context.Request.Headers.Add("X-UserId", authResponse.Data.UserId.ToString());
                context.Request.Headers.Add("X-Email", authResponse.Data.Email.ToString());
                context.Request.Headers.Add("X-Role", authResponse.Data.Role.ToString());
                context.Request.Headers.Add("X-UserName", authResponse.Data.UserName.ToString());
            }
            catch
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Invalid token.");
                return;
            }

            await _next(context);
        }
    }
}
