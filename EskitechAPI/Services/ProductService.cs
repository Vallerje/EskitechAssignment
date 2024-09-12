using Microsoft.EntityFrameworkCore;
using EskitechAPI.Data;


namespace EskitechAPI.Services
{
    public class ProductService
    {
        private readonly EskitechContext _context;

        public ProductService(EskitechContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }
        
        // Hämta en produkt baserat på dess ID
        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        }

        // Lägg till en ny produkt
        public async Task<Product> AddProductAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        // Uppdatera en befintlig produkt
        public async Task<bool> UpdateProductAsync(int id, Product product)
        {
            if (id != product.Id)
            {
                return false; // Returnerar false om ID inte matchar
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
                    return false; // Produkt hittades inte
                }
                throw; // Om annat fel, kasta undantaget vidare
            }
        }

        // Ta bort en produkt baserat på dess ID
        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return false; // Produkt hittades inte
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}