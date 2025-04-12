using Auth.API.Manager.Interface;
using Auth.WebAPI.Domain.Dto.Login;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Auth.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginManager _login;

        public LoginController(ILoginManager login)
        {
            _login = login;
        }

        [AllowAnonymous]
        [HttpPost("UserLogin")]
        public async Task<IActionResult> UserLogin(LoginRequestDto dto)
        {
            var userAgent = Request.Headers["User-Agent"].ToString();
            var userIp = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                userIp = Request.Headers["X-Forwarded-For"];

            var response = await _login.Login(dto, userAgent, userIp);
            return StatusCode(response.StatusCode, response);
        }

        [AllowAnonymous]
        [HttpPost("ValidateToken")]
        public async Task<IActionResult> ValidateToken([FromBody] string jwtToken)
        {
            var response = await _login.ValidateToken(jwtToken);
            return StatusCode(response.StatusCode, response);
        }

        [AllowAnonymous]
        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshJwtToken(AccessTokenFromRefreshTokenDto dto)
        {
            var response = await _login.RefreshJwtToken(dto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("UserLogout")]
        public async Task<IActionResult> Logout(LogoutRequestDto dto)
        {
            var response = await _login.Logout(dto);
            return StatusCode(response.StatusCode, response);
        }
    }
}
