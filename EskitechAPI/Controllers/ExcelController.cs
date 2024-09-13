using Microsoft.AspNetCore.Mvc;
using EskitechAPI.Services;

namespace EskitechAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExcelController : ControllerBase
    {
        private readonly ExcelService _service;

        public ExcelController(ExcelService excelService)
        {
            _service = excelService;
        }

        /// <summary>
        /// Endpoint to import data from an uploaded Excel file.
        /// </summary>
        /// <param name="file">The uploaded Excel file.</param>
        /// <returns>An IActionResult representing the result of the import operation.</returns>
        [HttpPost("excel")]
        public async Task<IActionResult> ImportExcelData(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded or the file is empty.");
            }

            try
            {
                // Save the file temporarily
                var filePath = Path.GetTempFileName();
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Call the import service to process the Excel file
                await _service.ImportDataFromExcelAsync(filePath);

                return Ok("Data successfully imported.");
            }
            catch (FileNotFoundException fnfEx)
            {
                // Handle specific exceptions first
                return NotFound(fnfEx.Message);
            }
            catch (InvalidOperationException ioeEx)
            {
                return BadRequest(ioeEx.Message);
            }
            catch (ApplicationException appEx)
            {
                return StatusCode(500, appEx.Message); // Internal Server Error
            }
            catch (Exception ex)
            {
                // Handle any unexpected exceptions
                return StatusCode(500, "An unexpected error occurred: " + ex.Message);
            }
        }
    }
}
