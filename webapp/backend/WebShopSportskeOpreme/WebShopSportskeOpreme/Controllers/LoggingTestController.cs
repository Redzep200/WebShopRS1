using Microsoft.AspNetCore.Mvc;

namespace WebShopSportskeOpreme.Controllers
{
    [Route("api/[controller]")]
    public class LoggingTestController : ControllerBase
    {
        [HttpGet("generate-error")]
        public IActionResult GenerateError()
        {
            throw new InvalidOperationException("This is a Serilog test.");
        }
    }
}
