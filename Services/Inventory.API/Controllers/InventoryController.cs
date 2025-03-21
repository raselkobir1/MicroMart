using Inventory.API.Domain.Dtos;
using Inventory.API.Manager.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryInfoManager _inventoryManager;
        public InventoryController(IInventoryInfoManager inventoryManager)
        {
            _inventoryManager = inventoryManager;
        }

        [HttpPost("Add")]
        public async Task<IActionResult> InventoryInfoAdd(InventoryInfoAddDto dto)
        {
            var response = await _inventoryManager.InventoryInfoAdd(dto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> InventoryInfoUpdate(InventoryInfoUpdateDto dto)
        {
            var response = await _inventoryManager.InventoryInfoUpdate(dto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> InventoryInfoDelete(long id)
        {
            var response = await _inventoryManager.InventoryInfoDelete(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("Get/{id}")]
        public async Task<IActionResult> InventoryInfoGetById(long id)
        {
            var response = await _inventoryManager.InventoryInfoGetById(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> InventoryInfoGetAll([FromQuery] InventoryInfoFilterDto dto)
        {
            var response = await _inventoryManager.InventoryInfoGetAll(dto);
            return StatusCode(response.StatusCode, response);
        }
    }
}
