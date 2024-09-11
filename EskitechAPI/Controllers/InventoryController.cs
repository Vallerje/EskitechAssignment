using Microsoft.AspNetCore.Mvc;
using EskitechAPI.Services;

namespace EskitechAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly InventoryService _inventoryService;

        public InventoryController(InventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        [HttpGet("{productId}")]
        public async Task<ActionResult<Inventory>> GetInventory(int productId)
        {
            var inventory = await _inventoryService.GetInventoryByProductIdAsync(productId);

            if (inventory == null)
            {
                return NotFound();
            }

            return Ok(inventory);
        }

        [HttpPost]
        public async Task<ActionResult<Inventory>> PostInventory(Inventory inventory)
        {
            var createdInventory = await _inventoryService.AddInventoryAsync(inventory);
            return CreatedAtAction(nameof(GetInventory), new { productId = createdInventory.ProductId }, createdInventory);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutInventory(int id, Inventory inventory)
        {
            var success = await _inventoryService.UpdateInventoryAsync(id, inventory);
            if (!success)
            {
                return BadRequest();
            }

            return NoContent();
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInventory(int id)
        {
            var success = await _inventoryService.DeleteInventoryAsync(id);
            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}