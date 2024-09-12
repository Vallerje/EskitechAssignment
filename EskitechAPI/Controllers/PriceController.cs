using Microsoft.AspNetCore.Mvc;
using EskitechAPI.Services;

namespace EskitechAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PriceController : ControllerBase
    {
        private readonly PriceService _priceService;

        // Constructor to inject PriceService dependency
        public PriceController(PriceService priceService)
        {
            _priceService = priceService;
        }

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

        [HttpPost]
        public async Task<ActionResult<Price>> PostPrice(Price price)
        {
            var createdPrice = await _priceService.AddPriceAsync(price);
            return CreatedAtAction(nameof(GetPrice), new { productId = createdPrice.ProductId }, createdPrice);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPrice(int id, Price price)
        {
            var success = await _priceService.UpdatePriceAsync(id, price);
            if (!success)
            {
                return BadRequest();
            }

            return NoContent();
        }
        
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