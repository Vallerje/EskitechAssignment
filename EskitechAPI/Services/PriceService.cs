using Microsoft.EntityFrameworkCore;
using EskitechAPI.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

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

            // Check if the ProductId exists in the Products table
            if (!await ProductExistsAsync(price.ProductId))
            {
                throw new KeyNotFoundException("Product with the given ProductId does not exist.");
            }

            _context.Prices.Add(price);
            await _context.SaveChangesAsync();
            return price;
        }

        // Method to delete a price by its Id
        public async Task<bool> DeletePriceAsync(int id)
        {
            var price = await _context.Prices.FindAsync(id);
            if (price == null)
            {
                return false; // Price not found
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