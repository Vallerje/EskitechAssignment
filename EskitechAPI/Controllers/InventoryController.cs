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

            try
            {
                var createdInventory = await _inventoryService.AddInventoryAsync(inventory);
                return CreatedAtAction(nameof(GetInventoryByIdAsync), new { id = createdInventory.Id }, createdInventory);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); // Return 404 if product not found
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message); // Return 400 if input is invalid
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message); // Return 500 for unexpected errors
            }
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
