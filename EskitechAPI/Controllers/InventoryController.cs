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
        
        //Hämtar alla inventories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Inventory>>> GetInventoriesAsync()
        {
            var products = await _inventoryService.GetInventoriesAsync();
            return Ok(products);
        }

        // Hämtar inventory genom productId
        [HttpGet("{productId}")]
        public async Task<ActionResult<Inventory>> GetInventoryAsync(int productId)
        {
            var inventory = await _inventoryService.GetInventoryByProductIdAsync(productId);

            if (inventory == null)
            {
                return NotFound();
            }

            return Ok(inventory);
        }

        
        // Lägger till ny inventory 
        [HttpPost]
        public async Task<ActionResult<Inventory>> PostInventoryAsync(Inventory inventory)
        {
            var createdInventory = await _inventoryService.AddInventoryAsync(inventory);
            return CreatedAtAction(nameof(GetInventoryAsync), new { productId = createdInventory.ProductId },
                createdInventory);
        }
        

        // Tar bort inventory
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInventoryAsync(int id)
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