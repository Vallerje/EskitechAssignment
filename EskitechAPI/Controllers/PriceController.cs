using Microsoft.AspNetCore.Mvc;
using EskitechAPI.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

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
            try
            {
                var price = await _priceService.GetPriceByIdAsync(id);

                if (price == null)
                {
                    return NotFound("Price with the given Id does not exist.");
                }

                return Ok(price);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving the price: {ex.Message}");
            }
        }

        // POST: api/Price
        [HttpPost]
        public async Task<ActionResult<Price>> PostPriceAsync([FromBody] Price price)
        {
            if (price == null)
            {
                return BadRequest("Price cannot be null.");
            }

            try
            {
                var createdPrice = await _priceService.AddPriceAsync(price);
                return CreatedAtAction(nameof(GetPriceByIdAsync), new { id = createdPrice.Id },
                    new { message = "Price created successfully.", price = createdPrice });
            }
            catch (DbUpdateException ex) when
                (ex.InnerException is SqliteException sqlEx && sqlEx.SqliteErrorCode == 19)
            {
                // SQLite error code 19 indicates a constraint violation
                return
                    Conflict(
                        "A price for this product already exists."); // Return 409 Conflict for unique constraint violations
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while adding the price: {ex.Message}");
            }
        }

        // DELETE: api/Price/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePriceAsync(int id)
        {
            try
            {
                var success = await _priceService.DeletePriceAsync(id);
                return Ok(new { Message = "Price successfully deleted." }); // Success message for deletion
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while deleting the price: {ex.Message}");
            }
        }
    }
}