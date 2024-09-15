using Microsoft.AspNetCore.Mvc;
using EskitechAPI.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EskitechAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PriceController : ControllerBase
    {
        private readonly PriceService _priceService;

        // Constructor to inject PriceService
        public PriceController(PriceService priceService)
        {
            _priceService = priceService;
        }

        // GET: api/Price
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Price>>> GetPricesAsync()
        {
            var prices = await _priceService.GetPricesAsync();
            return Ok(prices);
        }

        // GET: api/Price/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Price>> GetPriceByIdAsync(int id)
        {
            var price = await _priceService.GetPriceByIdAsync(id);

            if (price == null)
            {
                return NotFound(); // Return 404 if price not found
            }

            return Ok(price);
        }

        // POST: api/Price
        [HttpPost]
        public async Task<ActionResult<Price>> PostPriceAsync([FromBody] Price price)
        {
            if (price == null)
            {
                return BadRequest("Price cannot be null."); // Return 400 if input is invalid
            }

            try
            {
                var createdPrice = await _priceService.AddPriceAsync(price);
                return CreatedAtAction(nameof(GetPriceByIdAsync), new { id = createdPrice.Id }, createdPrice);
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

        // DELETE: api/Price/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePriceAsync(int id)
        {
            var success = await _priceService.DeletePriceAsync(id);
            if (!success)
            {
                return NotFound(); // Return 404 if price to delete not found
            }

            return NoContent(); // Return 204 No Content on successful deletion
        }
    }
}
