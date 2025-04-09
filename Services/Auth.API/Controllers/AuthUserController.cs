using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Auth.API.Domain.Dtos;
using Auth.API.Manager.Interface;

namespace Auth.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthUserController : ControllerBase
    {
        private readonly IUserManager _userManager;
        public AuthUserController(IUserManager UserManager)
        {
            _userManager = UserManager;
        }
        [HttpPost("Add")]
        public async Task<IActionResult> AuthUserAdd(UserAddDto dto)
        {
            var response = await _userManager.UserAdd(dto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> AuthUserUpdate(UserUpdateDto dto)
        {
            var response = await _userManager.UserUpdate(dto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> AuthUserDelete(long id)
        {
            var response = await _userManager.UserDelete(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("Get/{id}")]
        public async Task<IActionResult> AuthUserGetById(long id)
        {
            var response = await _userManager.UserGetById(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> AuthUserGetAll([FromQuery] UserFilterDto dto)
        {
            var response = await _userManager.UserGetAll(dto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("Verification")]
        public async Task<IActionResult> AuthUserVerification(UserVerificationDto dto)
        {
            var response = await _userManager.AuthUserVerification(dto);
            return StatusCode(response.StatusCode, response);
        }
    }
}
