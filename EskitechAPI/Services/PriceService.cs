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

        public async Task<Price> GetPriceByProductIdAsync(int productId)
        {
            return await _context.Prices.FirstOrDefaultAsync(p => p.ProductId == productId);
        }

        public async Task<Price> AddPriceAsync(Price price)
        {
            _context.Prices.Add(price);
            await _context.SaveChangesAsync();
            return price;
        }

        public async Task<bool> UpdatePriceAsync(int id, Price price)
        {
            if (id != price.Id)
            {
                return false;
            }

            _context.Entry(price).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Prices.AnyAsync(e => e.Id == id))
                {
                    return false;
                }
                throw;
            }
        }

        public async Task<bool> DeletePriceAsync(int id)
        {
            var price = await _context.Prices.FindAsync(id);
            if (price == null)
            {
                return false;
            }

            _context.Prices.Remove(price);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}