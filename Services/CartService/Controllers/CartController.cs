using Cart.API.Domain.Dtos;
using Cart.API.Manager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cart.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly IRedisCartService _cartService;
        public CartController(IRedisCartService redisCartService)
        {
            _cartService = redisCartService;   
        }

        [HttpPost("AddToCart")]
        public async Task<IActionResult> AddToCart([FromHeader(Name = "x-cart-session-id")] string? sessionId, [FromBody] CartItemDto item)
        {
            var response = await _cartService.AddToCartAsync(sessionId, item);
             HttpContext.Response.Headers["x-cart-session-id"] = response?.Data?.ToString();
            return StatusCode(response.StatusCode, response); 
        }
    }
}
