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

        
        //hämtar alla priser
        public async Task<IEnumerable<Price>> GetPricesAsync()
        {
            return await _context.Prices.ToListAsync();
        }
        
        
// Method to get an price by Id
        public async Task<Price> GetPriceByIdAsync(int id)
        {
            return await _context.Prices.FindAsync(id);
        }


        // Lägg till ett nytt pris
        public async Task<Price> AddPriceAsync(Price price)
        {
            _context.Prices.Add(price);
            await _context.SaveChangesAsync();
            return price;
        }


        // Ta bort ett pris baserat på dess ID
        public async Task<bool> DeletePriceAsync(int id)
        {
            var price = await _context.Prices.FindAsync(id);
            if (price == null)
            {
                return false; // Pris hittades inte
            }

            _context.Prices.Remove(price);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}