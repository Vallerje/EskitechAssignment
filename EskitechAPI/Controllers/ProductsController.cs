using Microsoft.AspNetCore.Mvc;
using EskitechAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace EskitechAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly EskitechContext _context;

        public ProductsController(EskitechContext context)
        {
            _context = context;
        }

        // GET: api/products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        // GET: api/products/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }
        
        // POST: api/products
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }
        
        // DELETE: api/products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        
        
        // GET: api/products/{id}/inventory
        [HttpGet("{id}/inventory")]
        public async Task<ActionResult<Inventory>> GetProductInventory(int id)
        {
            var inventory = await _context.Inventories.FirstOrDefaultAsync(i => i.ProductId == id);

            if (inventory == null)
            {
                return NotFound();
            }

            return inventory;
        }

        // GET: api/products/{id}/price
        [HttpGet("{id}/price")]
        public async Task<ActionResult<Price>> GetProductPrice(int id)
        {
            var price = await _context.Prices.FirstOrDefaultAsync(p => p.ProductId == id);

            if (price == null)
            {
                return NotFound();
            }

            return price;
        }

        // Other CRUD methods (POST, PUT, DELETE) for Product, Inventory, and Price can be added similarly
    }
}