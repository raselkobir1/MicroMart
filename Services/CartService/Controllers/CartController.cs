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

        [HttpGet("GetCart/{sessionId}")]
        public async Task<IActionResult> GetCart(string? sessionId)
        {
            var response = await _cartService.GetCartItemsAsync(sessionId);
            return StatusCode(response.StatusCode, response);
        }
        [HttpDelete("RemoveFromCart/{productId}")]
        public async Task<IActionResult> RemoveFromCart([FromHeader(Name = "x-cart-session-id")] string? sessionId, string productId)
        {
            var response = await _cartService.RemoveCartItemAsync(sessionId, productId);
            HttpContext.Response.Headers["x-cart-session-id"] = string.Empty;
            HttpContext.Request.Headers["x-cart-session-id"] = string.Empty;
            return StatusCode(response.StatusCode, response);
        }
    }
}
