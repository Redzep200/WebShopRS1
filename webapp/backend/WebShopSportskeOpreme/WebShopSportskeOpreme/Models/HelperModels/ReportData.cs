public class ReportData
{
    public int ProductNumber { get; set; }
    public int UserNumber { get; set; }
    public int CategoryNumber { get; set; }
    public string MostRecentUser { get; set; }
    public string CategoryWithMostProducts { get; set; }
    public int ProductCountInTopCategory { get; set; }
    public string BestRatedProductName { get; set; }
    public double BestRatedProductRating { get; set; }
    public string MostRecentCommentUser { get; set; }
    public string MostRecentCommentProduct { get; set; }
    public DateTime MostRecentCommentDate { get; set; }
    public int MostRecentCommentRating { get; set; }
    public string MostRecentCommentContent { get; set; }
}