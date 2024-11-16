using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using System.IO;
using System.Globalization;
using WebShopSportskeOpreme.Interfaces;

namespace WebShopSportskeOpreme.Services
{
    public class ReportService : IReportService
    {
        private readonly IUserService _userService;
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly ICommentService _commentService;
        public ReportService(IUserService userService, IProductService productService, 
            ICategoryService categoryService, ICommentService commentService)
        {
            _userService = userService;
            _productService = productService;
            _categoryService = categoryService;
            _commentService = commentService;
        }
        public byte[] GeneratePdfReport()
        {
            var productNumber = _productService.GetAllProducts().Count;
            var userNumber = _userService.GetAllUsers().Count;
            var categoryNumber = _categoryService.GetAllCategories().Count;

            var mostRecentUser = _userService.GetAllUsers().OrderByDescending(x => x.RegistrationDate).FirstOrDefault();
            var categoryWithMostProducts = _productService.GetAllProducts()
                .GroupBy(x => x.Category.Name)
                .OrderByDescending(x => x.Count())
                .Select(x => new { Category = x.Key, ProductCount = x.Count() })
                .FirstOrDefault();

            var bestRatedProductInfo = _commentService.GetAllComments()
                .GroupBy(x => x.ProductId)
                .Select(x => new
                {
                    ProductId = x.Key,
                    AverageRating = x.Average(y => y.ReviewRating)
                })
                .OrderByDescending(x => x.AverageRating)
                .FirstOrDefault();
            string bestRatedProductName = "";
            if(bestRatedProductInfo != null)
            {
                bestRatedProductName = _productService.GetProductById(bestRatedProductInfo.ProductId).Name;
            }
            var mostRecentComment = _commentService.GetAllComments()
                .OrderByDescending(x => x.Date)
                .Select(x=> new
                {
                    Date = x.Date,
                    User = x.User.Username,
                    ProductName = x.Product.Name,
                    CommentContent = x.Text,
                    CommentRating = x.ReviewRating
                })
                .FirstOrDefault();

            var dateOfReport = DateTime.Now;



            PdfDocument document = new PdfDocument();
            document.Info.Title = "Generated Report";

            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);

            XFont titleFont = new XFont("Times New Roman", 20, XFontStyle.Bold);
            XFont headerFont = new XFont("Times New Roman", 16, XFontStyle.Bold);
            XFont contentFont = new XFont("Times New Roman", 12, XFontStyle.Regular);

            double yPoint = 0;

            // Report Title
            gfx.DrawString("Report Summary", titleFont, XBrushes.Black, new XRect(0, yPoint, page.Width, page.Height), XStringFormats.TopCenter);
            yPoint += 40;

            // Date of Report
            gfx.DrawString("Date of Report: " + dateOfReport.ToString("MMMM dd, yyyy", CultureInfo.InvariantCulture), contentFont, XBrushes.Black,
                new XRect(40, yPoint, page.Width - 80, page.Height),
                XStringFormats.TopLeft);
            yPoint += 30;

            // Product, User, and Category Counts
            gfx.DrawString($"Total Number of Products: {productNumber}", contentFont, XBrushes.Black,
                new XRect(40, yPoint, page.Width - 80, page.Height),
                XStringFormats.TopLeft);
            yPoint += 20;

            gfx.DrawString($"Total Number of Users: {userNumber}", contentFont, XBrushes.Black,
                new XRect(40, yPoint, page.Width - 80, page.Height),
                XStringFormats.TopLeft);
            yPoint += 20;

            gfx.DrawString($"Total Number of Categories: {categoryNumber}", contentFont, XBrushes.Black,
                new XRect(40, yPoint, page.Width - 80, page.Height),
                XStringFormats.TopLeft);
            yPoint += 30;

            // Most Recent User
            if (mostRecentUser != null)
            {
                gfx.DrawString($"Most Recent User: {mostRecentUser.Username} (Joined on {mostRecentUser.RegistrationDate.Date.ToString("MMMM dd, yyyy",CultureInfo.InvariantCulture)})", contentFont, XBrushes.Black,
                    new XRect(40, yPoint, page.Width - 80, page.Height),
                    XStringFormats.TopLeft);
                yPoint += 30;
            }

            // Category with Most Products
            if (categoryWithMostProducts != null)
            {
                gfx.DrawString($"Category with Most Products: {categoryWithMostProducts.Category} ({categoryWithMostProducts.ProductCount} products)", contentFont, XBrushes.Black,
                    new XRect(40, yPoint, page.Width - 80, page.Height),
                    XStringFormats.TopLeft);
                yPoint += 30;
            }

            // Best Rated Product
            if (bestRatedProductName != null)
            {
                gfx.DrawString($"Best Rated Product: {bestRatedProductName} (Average Rating: {bestRatedProductInfo.AverageRating:F2})", contentFont, XBrushes.Black,
                    new XRect(40, yPoint, page.Width - 80, page.Height),
                    XStringFormats.TopLeft);
                yPoint += 30;
            }

            // Most Recent Comment
            if (mostRecentComment != null)
            {
                gfx.DrawString($"Most Recent Comment:", headerFont, XBrushes.Black,
                    new XRect(40, yPoint, page.Width - 80, page.Height),
                    XStringFormats.TopLeft);
                yPoint += 20;

                gfx.DrawString($"User: {mostRecentComment.User}", contentFont, XBrushes.Black,
                    new XRect(60, yPoint, page.Width - 100, page.Height),
                    XStringFormats.TopLeft);
                yPoint += 20;

                gfx.DrawString($"Product: {mostRecentComment.ProductName}", contentFont, XBrushes.Black,
                    new XRect(60, yPoint, page.Width - 100, page.Height),
                    XStringFormats.TopLeft);
                yPoint += 20;

                gfx.DrawString($"Date: {mostRecentComment.Date.ToString("MMMM dd, yyyy", CultureInfo.InvariantCulture)}", contentFont, XBrushes.Black,
                    new XRect(60, yPoint, page.Width - 100, page.Height),
                    XStringFormats.TopLeft);
                yPoint += 20;

                gfx.DrawString($"Rating: {mostRecentComment.CommentRating}/5", contentFont, XBrushes.Black,
                    new XRect(60, yPoint, page.Width - 100, page.Height),
                    XStringFormats.TopLeft);
                yPoint += 20;

                gfx.DrawString($"Comment: {mostRecentComment.CommentContent}", contentFont, XBrushes.Black,
                    new XRect(60, yPoint, page.Width - 100, page.Height),
                    XStringFormats.TopLeft);
                yPoint += 40;
            }
            using (MemoryStream stream = new MemoryStream())
            {
                document.Save(stream, false);
                return stream.ToArray();
            }
        }

        public ReportData GetReportData()
        {
            var productNumber = _productService.GetAllProducts().Count;
            var userNumber = _userService.GetAllUsers().Count;
            var categoryNumber = _categoryService.GetAllCategories().Count;

            var mostRecentUser = _userService.GetAllUsers().OrderByDescending(x => x.RegistrationDate).FirstOrDefault();
            var categoryWithMostProducts = _productService.GetAllProducts()
                .GroupBy(x => x.Category.Name)
                .OrderByDescending(x => x.Count())
                .Select(x => new { Category = x.Key, ProductCount = x.Count() })
                .FirstOrDefault();

            var bestRatedProductInfo = _commentService.GetAllComments()
                .GroupBy(x => x.ProductId)
                .Select(x => new
                {
                    ProductId = x.Key,
                    AverageRating = x.Average(y => y.ReviewRating)
                })
                .OrderByDescending(x => x.AverageRating)
                .FirstOrDefault();

            string bestRatedProductName = "";
            if (bestRatedProductInfo != null)
            {
                bestRatedProductName = _productService.GetProductById(bestRatedProductInfo.ProductId).Name;
            }

            var mostRecentComment = _commentService.GetAllComments()
                .OrderByDescending(x => x.Date)
                .Select(x => new
                {
                    Date = x.Date,
                    User = x.User.Username,
                    ProductName = x.Product.Name,
                    CommentContent = x.Text,
                    CommentRating = x.ReviewRating
                })
                .FirstOrDefault();

            return new ReportData
            {
                ProductNumber = productNumber,
                UserNumber = userNumber,
                CategoryNumber = categoryNumber,
                MostRecentUser = mostRecentUser?.Username,
                CategoryWithMostProducts = categoryWithMostProducts?.Category,
                ProductCountInTopCategory = categoryWithMostProducts?.ProductCount ?? 0,
                BestRatedProductName = bestRatedProductName,
                BestRatedProductRating = bestRatedProductInfo?.AverageRating ?? 0,
                MostRecentCommentUser = mostRecentComment?.User,
                MostRecentCommentProduct = mostRecentComment?.ProductName,
                MostRecentCommentDate = mostRecentComment?.Date ?? DateTime.MinValue,
                MostRecentCommentRating = mostRecentComment?.CommentRating ?? 0,
                MostRecentCommentContent = mostRecentComment?.CommentContent
            };
        }
    }
}
