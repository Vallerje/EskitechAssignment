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

        // Hämta pris baserat på produkt-ID
        public async Task<Price> GetPriceByProductIdAsync(int productId)
        {
            return await _context.Prices.FirstOrDefaultAsync(p => p.ProductId == productId);
        }

        // Lägg till ett nytt pris
        public async Task<Price> AddPriceAsync(Price price)
        {
            _context.Prices.Add(price);
            await _context.SaveChangesAsync();
            return price;
        }

        // Uppdatera ett befintligt pris
        public async Task<bool> UpdatePriceAsync(int id, Price price)
        {
            if (id != price.Id)
            {
                return false; // Returnerar false om ID inte matchar
            }

            _context.Entry(price).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Prices.AnyAsync(p => p.Id == id))
                {
                    return false; // Pris hittades inte
                }
                throw; // Om annat fel, kasta undantaget vidare
            }
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