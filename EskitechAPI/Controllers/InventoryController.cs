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
        public async Task<ActionResult<Inventory>> GetInventoryByIdAsync(int id)
        {
            try
            {
                var inventory = await _inventoryService.GetInventoryByIdAsync(id);

                if (inventory == null)
                {
                    return NotFound("Inventory with the given Id does not exist.");
                }

                return Ok(inventory);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving the inventory: {ex.Message}");
            }
        }

        // POST: api/Inventory
        [HttpPost]
        public async Task<ActionResult<Inventory>> PostInventoryAsync([FromBody] Inventory inventory)
        {
            if (inventory == null)
            {
                return BadRequest("Inventory cannot be null.");
            }

            try
            {
                var createdInventory = await _inventoryService.AddInventoryAsync(inventory);
                return CreatedAtAction(nameof(GetInventoryByIdAsync), new { id = createdInventory.Id }, createdInventory);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while adding the inventory: {ex.Message}");
            }
        }

        // DELETE: api/Inventory/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInventoryAsync(int id)
        {
            try
            {
                var success = await _inventoryService.DeleteInventoryAsync(id);
                return NoContent(); // Return 204 No Content on successful deletion
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while deleting the inventory: {ex.Message}");
            }
        }
    }
}
