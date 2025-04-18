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
            var response = await _orderManager.OrderCheckout(dto);
            return StatusCode(response.StatusCode, response);
        }
    }
}
