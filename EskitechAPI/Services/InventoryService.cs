using Microsoft.EntityFrameworkCore;
using EskitechAPI.Data;

namespace EskitechAPI.Services
{
    public class InventoryService
    {
        private readonly EskitechContext _context;

        public InventoryService(EskitechContext context)
        {
            _context = context;
        }
        
        
        //hämtar alla inventories
        public async Task<IEnumerable<Inventory>> GetInventoriesAsync()
        {
            return await _context.Inventories.ToListAsync();
        }


        // Hämta inventory baserat på dess ID
        public async Task<Inventory> GetInventoryByProductIdAsync(int productId)
        {
            return await _context.Inventories.FirstOrDefaultAsync(i => i.ProductId == productId);
        }

        // Lägg till en ny inventory
        public async Task<Inventory> AddInventoryAsync(Inventory inventory)
        {
            _context.Inventories.Add(inventory);
            await _context.SaveChangesAsync();
            return inventory;
        }

        // Ta bort ett inventory baserat på dess ID
        public async Task<bool> DeleteInventoryAsync(int id)
        {
            var inventory = await _context.Inventories.FindAsync(id);
            if (inventory == null)
            {
                return false; // Inventory hittades inte
            }

            _context.Inventories.Remove(inventory);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}