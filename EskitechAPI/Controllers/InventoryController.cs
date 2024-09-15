using Microsoft.AspNetCore.Mvc;
using EskitechAPI.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EskitechAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly InventoryService _inventoryService;

        // Constructor to inject InventoryService
        public InventoryController(InventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        // GET: api/Inventory
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Inventory>>> GetInventoriesAsync()
        {
            var inventories = await _inventoryService.GetInventoriesAsync();
            return Ok(inventories);
        }

// GET: api/Inventory/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Inventory>> GetInventoryAsync(int id)
        {
            var inventory = await _inventoryService.GetInventoryByIdAsync(id);

            if (inventory == null)
            {
                return NotFound(); // Return 404 if inventory not found
            }

            return Ok(inventory);
        }


// POST: api/Inventory
        [HttpPost]
        public async Task<ActionResult<Inventory>> PostInventoryAsync([FromBody] Inventory inventory)
        {
            if (inventory == null)
            {
                return BadRequest("Inventory cannot be null."); // Return 400 if input is invalid
            }

            // Check if the ProductId exists in the Products table
            var productExists = await _inventoryService.ProductExistsAsync(inventory.ProductId);
            if (!productExists)
            {
                return NotFound("Product with the given ProductId does not exist."); // Return 404 if product not found
            }

            var createdInventory = await _inventoryService.AddInventoryAsync(inventory);

            // Return the created Inventory with a link to its retrieval endpoint
            return CreatedAtAction(nameof(GetInventoryAsync), new { id = createdInventory.Id }, createdInventory);
        }


        // DELETE: api/Inventory/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInventoryAsync(int id)
        {
            var success = await _inventoryService.DeleteInventoryAsync(id);
            if (!success)
            {
                return NotFound(); // Return 404 if inventory to delete not found
            }

            return NoContent(); // Return 204 No Content on successful deletion
        }
    }
}
