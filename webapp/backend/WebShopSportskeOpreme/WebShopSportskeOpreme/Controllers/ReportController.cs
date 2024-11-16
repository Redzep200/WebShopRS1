using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebShopSportskeOpreme.Interfaces;

namespace WebShopSportskeOpreme.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ReportController : Controller
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("pdf")]
        public IActionResult GetPdfReport()
        {
            var pdfBytes = _reportService.GeneratePdfReport();
            return File(pdfBytes, "application/pdf", "report.pdf");
        }
        [HttpGet("dashboard-data")]
        public ActionResult<ReportData> GetDashboardData()
        {
            var reportData = _reportService.GetReportData();
            return Ok(reportData);
        }
    }
}
