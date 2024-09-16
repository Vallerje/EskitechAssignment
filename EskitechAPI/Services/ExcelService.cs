using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using OfficeOpenXml; 
using EskitechAPI.Data;

namespace EskitechAPI.Services
{
    public class ExcelService
    {
        private readonly EskitechContext _context;

        public ExcelService(EskitechContext context)
        {
            _context = context;
        }

        
        //importerar data från en excel fil in till database
    public async Task ImportDataFromExcelAsync(string filePath)
{
    if (!File.Exists(filePath))
        throw new FileNotFoundException("Excel file not found.", filePath);

    var products = new List<Product>();
    var inventories = new List<Inventory>();
    var prices = new List<Price>();

    using (var package = new ExcelPackage(new FileInfo(filePath)))
    {
        ExcelWorksheet productSheet = package.Workbook.Worksheets["Products"];
        ExcelWorksheet inventorySheet = package.Workbook.Worksheets["Inventories"];
        ExcelWorksheet priceSheet = package.Workbook.Worksheets["Prices"];

        if (productSheet == null || inventorySheet == null || priceSheet == null)
            throw new InvalidOperationException("Required worksheets are missing in the Excel file.");

        // Create a dictionary to map product names to product IDs
        var productMap = new Dictionary<string, int>();

        // Read and save product data first
        for (int row = 2; row <= productSheet.Dimension.End.Row; row++)
        {
            var product = new Product
            {
                Name = productSheet.Cells[row, 1].Text,
                Description = productSheet.Cells[row, 2].Text
            };

            products.Add(product);
        }

        // Save products to the database first
        _context.Products.AddRange(products);
        await _context.SaveChangesAsync();

        // After saving, map product names to their generated IDs
        foreach (var product in products)
        {
            productMap[product.Name] = product.Id;  // Assuming Product has an Id property that's auto-generated
        }

        // Read and save inventory data
        for (int row = 2; row <= inventorySheet.Dimension.End.Row; row++)
        {
            string productName = inventorySheet.Cells[row, 2].Text; // Assuming product name is used instead of ID
            if (productMap.ContainsKey(productName))
            {
                inventories.Add(new Inventory
                {
                    ProductId = productMap[productName], // Use the mapped product ID
                    Quantity = int.Parse(inventorySheet.Cells[row, 3].Text)
                });
            }
        }

        // Read and save price data
        for (int row = 2; row <= priceSheet.Dimension.End.Row; row++)
        {
            string productName = priceSheet.Cells[row, 2].Text; // Assuming product name is used instead of ID
            if (productMap.ContainsKey(productName))
            {
                prices.Add(new Price
                {
                    ProductId = productMap[productName], // Use the mapped product ID
                    Amount = decimal.Parse(priceSheet.Cells[row, 3].Text)
                });
            }
        }
    }

            // Sparar data till databasen
            await SaveDataToDatabaseAsync(products, inventories, prices);
        }
        
        //sparar viktig data till databasen
        private async Task SaveDataToDatabaseAsync(List<Product> products, List<Inventory> inventories, List<Price> prices)
        {
            try
            {
                _context.Products.AddRange(products);
                _context.Inventories.AddRange(inventories);
                _context.Prices.AddRange(prices);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log exception here or handle it accordingly
                throw new ApplicationException("An error occurred while saving data to the database.", ex);
            }
        }
    }
}
