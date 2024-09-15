using Microsoft.EntityFrameworkCore;
using EskitechAPI.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EskitechAPI.Services
{
    public class InventoryService
    {
        private readonly EskitechContext _context;

        // Constructor to inject EskitechContext
        public InventoryService(EskitechContext context)
        {
            _context = context;
        }

        // Method to get all inventories
        public async Task<IEnumerable<Inventory>> GetInventoriesAsync()
        {
            return await _context.Inventories.ToListAsync();
        }
        
// Method to get an inventory by Id
        public async Task<Inventory> GetInventoryByIdAsync(int id)
        {
            return await _context.Inventories.FindAsync(id);
        }


        // Method to add a new inventory
        public async Task<Inventory> AddInventoryAsync(Inventory inventory)
        {
            _context.Inventories.Add(inventory);
            await _context.SaveChangesAsync();
            return inventory;
        }

        // Method to delete an inventory by its Id
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

        // Method to check if a Product exists
        public async Task<bool> ProductExistsAsync(int productId)
        {
            return await _context.Products.AnyAsync(p => p.Id == productId);
        }
    }
}