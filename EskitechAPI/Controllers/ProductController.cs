using Microsoft.AspNetCore.Mvc;
using EskitechAPI.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EskitechAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;

        // Constructor to inject ProductService
        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        // GET: api/Product
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsAsync()
        {
            var products = await _productService.GetProductsAsync();
            return Ok(products);
        }

        // GET: api/Product/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductByIdAsync(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);

            if (product == null)
            {
                return NotFound(); // Return 404 if product not found
            }

            return Ok(product);
        }

        // POST: api/Product
        [HttpPost]
        public async Task<ActionResult<Product>> PostProductAsync([FromBody] Product product)
        {
            if (product == null)
            {
                return BadRequest("Product cannot be null."); // Return 400 if input is invalid
            }

            try
            {
                var createdProduct = await _productService.AddProductAsync(product);
                return CreatedAtAction(nameof(GetProductByIdAsync), new { id = createdProduct.Id }, createdProduct);
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

        // PUT: api/Product/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductAsync(int id, [FromBody] Product product)
        {
            if (product == null || id != product.Id)
            {
                return BadRequest("Product data is invalid."); // Return 400 if input is invalid
            }

            try
            {
                var success = await _productService.UpdateProductAsync(id, product);
                if (!success)
                {
                    return NotFound(); // Return 404 if product to update not found
                }
                return NoContent(); // Return 204 No Content on successful update
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message); // Return 500 for unexpected errors
            }
        }

        // DELETE: api/Product/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductAsync(int id)
        {
            var success = await _productService.DeleteProductAsync(id);
            if (!success)
            {
                return NotFound(); // Return 404 if product to delete not found
            }

            return NoContent(); // Return 204 No Content on successful deletion
        }
    }
}
