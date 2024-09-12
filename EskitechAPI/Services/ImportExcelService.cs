using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using OfficeOpenXml; // EPPlus namespace
using EskitechAPI.Data;

namespace EskitechAPI.Services
{
    public class ImportExcelService
    {
        private readonly EskitechContext _context;

        public ImportExcelService(EskitechContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Imports data from an Excel file to the database.
        /// </summary>
        /// <param name="filePath">The path to the Excel file.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task ImportDataFromExcelAsync(string filePath)
        {
            // Ensure the file exists
            if (!File.Exists(filePath))
                throw new FileNotFoundException("Excel file not found.", filePath);

            var products = new List<Product>();
            var inventories = new List<Inventory>();
            var prices = new List<Price>();

            // Read data from the Excel file
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                ExcelWorksheet productSheet = package.Workbook.Worksheets["Products"];
                /*ExcelWorksheet inventorySheet = package.Workbook.Worksheets["Inventories"];
                ExcelWorksheet priceSheet = package.Workbook.Worksheets["Prices"];*/

                // Validate that worksheets exist
                if (productSheet == null /*|| inventorySheet == null || priceSheet == null*/)
                    throw new InvalidOperationException("Required worksheets are missing in the Excel file.");

                // Read product data
                for (int row = 2; row <= productSheet.Dimension.End.Row; row++)
                {
                    products.Add(new Product
                    {
                        Name = productSheet.Cells[row, 1].Text,
                        Description = productSheet.Cells[row, 2].Text
                    });
                }

                /*// Read inventory data
                for (int row = 2; row <= inventorySheet.Dimension.End.Row; row++)
                {
                    inventories.Add(new Inventory
                    {
                        ProductId = int.Parse(inventorySheet.Cells[row, 1].Text),
                        Quantity = int.Parse(inventorySheet.Cells[row, 2].Text)
                    });
                }

                // Read price data
                for (int row = 2; row <= priceSheet.Dimension.End.Row; row++)
                {
                    prices.Add(new Price
                    {
                        ProductId = int.Parse(priceSheet.Cells[row, 1].Text),
                        Amount = decimal.Parse(priceSheet.Cells[row, 2].Text)
                    });
                }*/
            }

            // Save data to the database
            await SaveDataToDatabaseAsync(products, inventories, prices);
        }

        /// <summary>
        /// Saves imported data to the database.
        /// </summary>
        /// <param name="products">The list of products.</param>
        /// <param name="inventories">The list of inventories.</param>
        /// <param name="prices">The list of prices.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
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
