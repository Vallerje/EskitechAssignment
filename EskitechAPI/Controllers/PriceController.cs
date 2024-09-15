using Microsoft.AspNetCore.Mvc;
using EskitechAPI.Services;

namespace EskitechAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PriceController : ControllerBase
    {
        private readonly PriceService _priceService;

        public PriceController(PriceService priceService)
        {
            _priceService = priceService;
        }
        
        //Hämtar alla priser
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetPricesAsync()
        {
            var products = await _priceService.GetPricesAsync();
            return Ok(products);
        }


// GET: api/Price/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Price>> GetPriceByIdAsync(int id)
        {
            var price = await _priceService.GetPriceByIdAsync(id);

            if (price == null)
            {
                return NotFound(); // Return 404 if inventory not found
            }

            return Ok(price);
        }

        // Lägger till nytt pris 
        [HttpPost]
        public async Task<ActionResult<Price>> PostPriceAsync(Price price)
        {
            var createdPrice = await _priceService.AddPriceAsync(price);
            return CreatedAtAction(nameof(GetPriceByIdAsync), new { productId = createdPrice.ProductId }, createdPrice);
        }

        // Tar bort pris
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePriceAsync(int id)
        {
            var success = await _priceService.DeletePriceAsync(id);
            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}