using Microsoft.AspNetCore.Mvc;
using EskitechAPI.Services;

namespace EskitechAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;
        
        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        //Hämtar alla produkter
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
                return NotFound();
            }

            return Ok(product);
        }

        // Lägger till ny produkt 
        [HttpPost]
        public async Task<ActionResult<Product>> PostProductAsync(Product product)
        {
            var createdProduct = await _productService.AddProductAsync(product);
            return CreatedAtAction(nameof(GetProductByIdAsync), new { id = createdProduct.Id }, createdProduct);
        }

        //uppdaterar befintlig produkt
        /*[HttpPut("{id}")]
        public async Task<IActionResult> PutProductAsync(int id, Product product)
        {
            var success = await _productService.UpdateProductAsync(id, product);
            if (!success)
            {
                return BadRequest();
            }

            return NoContent();
        }*/

        // Tar bort produkt
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductAsync(int id)
        {
            var success = await _productService.DeleteProductAsync(id);
            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}