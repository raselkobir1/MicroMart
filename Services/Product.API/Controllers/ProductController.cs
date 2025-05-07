using Microsoft.AspNetCore.Mvc;
using Product.API.Domain.Dtos;
using Product.API.Manager.Interface;

namespace Product.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductManager _productManager;
        public ProductController(IProductManager productManager)
        {
            _productManager = productManager;     
        }
        [HttpPost("Add")]
        public async Task<IActionResult> ProductAdd(ProductAddDto dto)
        {
            var response = await _productManager.ProductAdd(dto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> ProductUpdate(ProductUpdateDto dto)
        {
            var response = await _productManager.ProductUpdate(dto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> ProductDelete(long id)
        {
            var response = await _productManager.ProductDelete(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("Get/{id}")]
        public async Task<IActionResult> ProductGetById(long id)
        {
            var response = await _productManager.ProductGetById(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> ProductGetAll([FromQuery] ProductFilterDto dto)
        {
            //var headerValue = HttpContext?.Request.Headers["x-user-role"].ToString();
            var response = await _productManager.ProductGetAll(dto);
            return StatusCode(response.StatusCode, response);
        }
    }
}
