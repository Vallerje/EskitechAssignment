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

        
        //Endpoint för att importera data från den uppladdade excel filen

        [HttpPost("importExcel")]
        public async Task<IActionResult> ImportExcelData(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded or the file is empty.");
            }

            try
            {
                // sparar filen temporärt
                var filePath = Path.GetTempFileName();
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                //    // kallar på import servicen för att bearbeta excel filen
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
