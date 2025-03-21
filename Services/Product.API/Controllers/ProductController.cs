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
        public async Task<IActionResult> InventoryInfoAdd(ProductAddDto dto)
        {
            var response = await _productManager.ProductAdd(dto);
            return StatusCode(response.StatusCode, response);
        }
    }
}
