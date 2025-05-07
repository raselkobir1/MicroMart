using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System.Net.Mail;
using System.Security.Claims;

namespace ApiGateway.Helper.Middleware
{
    public class AddUserHeadersDelegatingHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AddUserHeadersDelegatingHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var context = _httpContextAccessor.HttpContext;

            if (context?.User?.Identity?.IsAuthenticated == true)
            {
                var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var username = context.User.FindFirst(ClaimTypes.Name)?.Value;
                var email = context.User.FindFirst(ClaimTypes.Email)?.Value;
                //var role =  context.User.FindFirst(ClaimTypes.Role)?.Value;

                // 🎯 Extract roles from resource_access
                var resourceAccess = context.User.FindFirst("resource_access");
                if (resourceAccess != null)
                {
                    // Extract the roles from the "account" resource access
                    var resourceAccessData = JObject.Parse(resourceAccess.Value.ToString());
                    var accountRoles = resourceAccessData["account"]?["roles"]?.ToObject<string[]>();

                    if (accountRoles != null && accountRoles.Length > 0)
                    {
                        // 🎯 Attach roles to headers
                        foreach (var role in accountRoles)
                        {
                            request.Headers.Add("x-user-role", role);
                        }
                    }
                }

                // 🎯 Attach to outgoing request headers
                if (!string.IsNullOrEmpty(userId))
                    request.Headers.Add("x-user-id", userId);

                if (!string.IsNullOrEmpty(username))
                    request.Headers.Add("x-user-name", username);

                if (!string.IsNullOrEmpty(email))
                    request.Headers.Add("x-user-email", email);

                //if (!string.IsNullOrEmpty(role))
                //    request.Headers.Add("x-user-role", role);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
