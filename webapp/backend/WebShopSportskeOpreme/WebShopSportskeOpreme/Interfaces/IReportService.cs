

namespace WebShopSportskeOpreme.Interfaces
{
    public interface IReportService
    {
        byte[] GeneratePdfReport();
        ReportData GetReportData();
    }
}
