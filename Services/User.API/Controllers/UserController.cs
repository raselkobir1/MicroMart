using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using User.API.Domain.Dtos;
using User.API.Manager.Interface;

namespace User.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserManager _userManager;
        public UserController(IUserManager UserManager)
        {
            _userManager = UserManager;
        }
        [HttpPost("Add")]
        public async Task<IActionResult> UserAdd(UserAddDto dto)
        {
            var response = await _userManager.UserAdd(dto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> UserUpdate(UserUpdateDto dto)
        {
            var response = await _userManager.UserUpdate(dto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> UserDelete(long id)
        {
            var response = await _userManager.UserDelete(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("Get/{id}")]
        public async Task<IActionResult> UserGetById(long id)
        {
            var response = await _userManager.UserGetById(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> UserGetAll([FromQuery] UserFilterDto dto)
        {
            var response = await _userManager.UserGetAll(dto);
            return StatusCode(response.StatusCode, response);
        }
    }
}
