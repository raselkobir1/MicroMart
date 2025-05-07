using ApiGateway.Dto;
using ApiGateway.Manager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KeyCloakAuthController : ControllerBase
    {
        private readonly IKeycloakAuthService _keycloakAuth;
        public KeyCloakAuthController(IKeycloakAuthService keycloakAuth)
        {
            _keycloakAuth = keycloakAuth;
        }

        [AllowAnonymous]
        [HttpPost("token")]
        public async Task<IActionResult> GetToken([FromBody] TokenRequestDto dto) 
        {
            // Redirect to Keycloak login page
            //var redirectUrl = "https://your-keycloak-server/auth/realms/your-realm/protocol/openid-connect/auth";
            var response = await _keycloakAuth.GetToken(dto);
            return StatusCode(200, response);
        }
    }
}
