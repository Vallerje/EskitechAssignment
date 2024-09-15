using Microsoft.EntityFrameworkCore;
using EskitechAPI.Data;

namespace EskitechAPI.Services
{
    public class PriceService
    {
        private readonly EskitechContext _context;

        public PriceService(EskitechContext context)
        {
            _context = context;
        }

        // Method to get all prices
        public async Task<IEnumerable<Price>> GetPricesAsync()
        {
            return await _context.Prices.ToListAsync();
        }

        // Method to get a price by Id
        public async Task<Price> GetPriceByIdAsync(int id)
        {
            return await _context.Prices.FindAsync(id);
        }

        // Method to add a new price
        public async Task<Price> AddPriceAsync(Price price)
        {
            if (price == null)
            {
                throw new ArgumentNullException(nameof(price), "Price cannot be null.");
            }

            // Ensure that the Product exists
            var productExists = await ProductExistsAsync(price.ProductId);
            if (!productExists)
            {
                throw new InvalidOperationException("Product with the given ProductId does not exist.");
            }

            _context.Prices.Add(price);
            await _context.SaveChangesAsync();
            return price;
        }

        // Method to delete a price by Id
        public async Task<bool> DeletePriceAsync(int id)
        {
            var price = await _context.Prices.FindAsync(id);
            if (price == null)
            {
                throw new KeyNotFoundException("Price with the given Id does not exist.");
            }

            _context.Prices.Remove(price);
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