using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Order.API.Domain.Dtos;
using Order.API.Manager.Interface;

namespace Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderManager _orderManager;
        public OrderController(IOrderManager orderManager)
        {
            _orderManager = orderManager; 
        }

        [HttpPost("checkout")]
        public async Task<IActionResult> OrderCheckout([FromBody] OrderAddDto dto)
        {
            if(string.IsNullOrEmpty(dto.UserName)){
                dto.UserName = HttpContext.Request.Headers["x-user-name"];
            }
            if (string.IsNullOrEmpty(dto.UserEmail))
            {
                dto.UserEmail = HttpContext.Request.Headers["x-user-email"];
            }
            if (dto.UserId == null || dto.UserId < 0)
            {
                dto.UserId = Convert.ToInt64( HttpContext.Request.Headers["x-user-id"]);
            }

            var response = await _orderManager.OrderCheckout(dto);
            return StatusCode(response.StatusCode, response);
        }
    }
}
