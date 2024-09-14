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
        public async Task<ActionResult<IEnumerable<Product>>> GetPrices()
        {
            var products = await _priceService.GetPricesAsync();
            return Ok(products);
        }


        // Hämtar pris genom productId
        [HttpGet("{productId}")]
        public async Task<ActionResult<Price>> GetPrice(int productId)
        {
            var price = await _priceService.GetPriceByProductIdAsync(productId);

            if (price == null)
            {
                return NotFound();
            }

            return Ok(price);
        }

        // Lägger till nytt pris 
        [HttpPost]
        public async Task<ActionResult<Price>> PostPrice(Price price)
        {
            var createdPrice = await _priceService.AddPriceAsync(price);
            return CreatedAtAction(nameof(GetPrice), new { productId = createdPrice.ProductId }, createdPrice);
        }

        // Tar bort pris
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePrice(int id)
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