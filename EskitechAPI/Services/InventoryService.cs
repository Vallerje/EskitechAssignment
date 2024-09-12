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

        // Fetch inventory by productId
        public async Task<Inventory> GetInventoryByProductIdAsync(int productId)
        {
            return await _context.Inventories.FirstOrDefaultAsync(i => i.ProductId == productId);
        }

        // Add a new inventory record
        public async Task<Inventory> AddInventoryAsync(Inventory inventory)
        {
            _context.Inventories.Add(inventory);
            await _context.SaveChangesAsync();
            return inventory;
        }

        // Update existing inventory
        public async Task<bool> UpdateInventoryAsync(int id, Inventory inventory)
        {
            if (id != inventory.Id)
            {
                return false; // Invalid ID
            }

            _context.Entry(inventory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Inventories.AnyAsync(e => e.Id == id))
                {
                    return false; // Inventory not found
                }

                throw; // Re-throw the exception to be handled at a higher level
            }
        }

        // Delete an inventory record
        public async Task<bool> DeleteInventoryAsync(int id)
        {
            var inventory = await _context.Inventories.FindAsync(id);
            if (inventory == null)
            {
                return false; // Inventory not found
            }

            _context.Inventories.Remove(inventory);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}