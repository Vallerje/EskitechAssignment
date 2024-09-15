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
            // Checkar att filen existerar
            if (!File.Exists(filePath))
                throw new FileNotFoundException("Excel file not found.", filePath);

            var products = new List<Product>();
            var inventories = new List<Inventory>();
            var prices = new List<Price>();

            // Läser data från excel filen
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                ExcelWorksheet productSheet = package.Workbook.Worksheets["Products"];
                ExcelWorksheet inventorySheet = package.Workbook.Worksheets["Inventories"];
                ExcelWorksheet priceSheet = package.Workbook.Worksheets["Prices"];

                // validerar att worksheets finns
                if (productSheet == null || inventorySheet == null || priceSheet == null)
                    throw new InvalidOperationException("Required worksheets are missing in the Excel file.");

                // Läser produkt data
                for (int row = 2; row <= productSheet.Dimension.End.Row; row++)
                {
                    products.Add(new Product
                    {
                        Name = productSheet.Cells[row, 1].Text,
                        Description = productSheet.Cells[row, 2].Text
                    });
                }

                // läser inventory data
                for (int row = 2; row <= inventorySheet.Dimension.End.Row; row++)
                {
                    inventories.Add(new Inventory
                    {
                        ProductId = int.Parse(inventorySheet.Cells[row, 1].Text),
                        Quantity = int.Parse(inventorySheet.Cells[row, 2].Text)
                    });
                }

                // läser pris data
                for (int row = 2; row <= priceSheet.Dimension.End.Row; row++)
                {
                    prices.Add(new Price
                    {
                        ProductId = int.Parse(priceSheet.Cells[row, 1].Text),
                        Amount = decimal.Parse(priceSheet.Cells[row, 2].Text)
                    });
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
