using Microsoft.AspNetCore.Mvc;
using EskitechAPI.Services;

namespace EskitechAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly InventoryService _inventoryService;

        // Constructor to inject InventoryService dependency
        public InventoryController(InventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        // GET: api/inventory/{productId}
        [HttpGet("{productId}")]
        public async Task<ActionResult<Inventory>> GetInventory(int productId)
        {
            // Fetch inventory by productId
            var inventory = await _inventoryService.GetInventoryByProductIdAsync(productId);

            if (inventory == null)
            {
                return NotFound();
            }

            return Ok(inventory);
        }

        // POST: api/inventory
        [HttpPost]
        public async Task<ActionResult<Inventory>> PostInventory(Inventory inventory)
        {
            // Add new inventory record
            var createdInventory = await _inventoryService.AddInventoryAsync(inventory);
            return CreatedAtAction(nameof(GetInventory), new { productId = createdInventory.ProductId }, createdInventory);
        }

        // PUT: api/inventory/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInventory(int id, Inventory inventory)
        {
            // Update inventory details
            var success = await _inventoryService.UpdateInventoryAsync(id, inventory);
            if (!success)
            {
                return BadRequest();
            }

            return NoContent();
        }
        
        // DELETE: api/inventory/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInventory(int id)
        {
            // Delete inventory record
            var success = await _inventoryService.DeleteInventoryAsync(id);
            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
