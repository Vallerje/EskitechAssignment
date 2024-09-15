using Microsoft.EntityFrameworkCore;
using EskitechAPI.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EskitechAPI.Services
{
    public class ProductService
    {
        private readonly EskitechContext _context;

        public ProductService(EskitechContext context)
        {
            _context = context;
        }

        // Method to get all products
        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }

        // Method to get a product by Id
        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        }

        // Method to add a new product
        public async Task<Product> AddProductAsync(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product), "Product cannot be null.");
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        // Method to update an existing product
        public async Task<bool> UpdateProductAsync(int id, Product product)
        {
            if (id != product.Id)
            {
                return false; // Return false if IDs do not match
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Products.AnyAsync(p => p.Id == id))
                {
                    return false; // Product not found
                }
                throw; // Re-throw other concurrency issues
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the product.", ex);
            }
        }

        // Method to delete a product by Id
        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return false; // Product not found
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }

        // Method to check if a Product exists
        public async Task<bool> ProductExistsAsync(int productId)
        {
            return await _context.Products.AnyAsync(p => p.Id == productId);
        }
    }
}
