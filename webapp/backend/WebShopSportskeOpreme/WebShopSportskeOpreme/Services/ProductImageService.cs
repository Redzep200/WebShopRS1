using WebShopSportskeOpreme.Interfaces;
using WebShopSportskeOpreme.Models;
using WebShopSportskeOpreme.Helpers;

namespace WebShopSportskeOpreme.Services
{
    public class ProductImageService : IProductImageService
    {

        private readonly WebShopDbContext _context;
        private readonly IProductService _productService;

        public ProductImageService(WebShopDbContext context, IProductService productService)
        {
            _context = context;
            _productService = productService;
        }

        public bool CreateProductImage(int productId, string image)
        {
            if (_context.Products.Where(x=>x.Id==productId) == null || String.IsNullOrEmpty(image))
                return false;

            int commaIndex = image.IndexOf(',');
            var format = image.Substring(0, commaIndex + 1);
            var imageString = image.Substring(commaIndex + 1);
            var img = new ProductImage()
            {
                ProductID = productId,
                ImageByteArray = imageString.parseBase64(),
                ImageFormat = format
            };
            if (_context.ProductImages.Where(x => x.ProductID == productId).Any())
            {
                var obj = _context.ProductImages.Where(x => x.ProductID == productId).FirstOrDefault();
                _context.ProductImages.Remove(obj);
            }
            _context.ProductImages.Add(img);
            
            _context.SaveChanges();
            return true;
        }

        public bool DeleteProductImageByProductId(int productId)
        {
            var img = _context.ProductImages.FirstOrDefault(x => x.ProductID == productId);
            if (img == null)
                return false;
            _context.ProductImages.Remove(img);
            _context.SaveChanges();
            return true;
        }

        public List<ProductImage> GetAllProductImages()
        {
            return _context.ProductImages.ToList();
        }

        public List<ProductImage> GetImagesByProductId(int productId)
        {
            return _context.ProductImages.Where(x => x.ProductID == productId).ToList();
        }

        public List<ProductImage> GetProductImagesByCategoryId(int categoryId)
        {
            var products = _productService.GetProductByCategory(categoryId);
            var list = new List<ProductImage>();
            foreach (Product item in products)
            {
                var prodImg = _context.ProductImages.Where(x=>x.ProductID == item.Id).First();
                if(prodImg != null)
                    list.Add(prodImg);
            }
            return list.Distinct().ToList();
        }

        public List<ProductImage> GetProductImagesByProductName(string name)
        {
            var list = new List<ProductImage>();
            if (String.IsNullOrEmpty(name))
                return _context.ProductImages.ToList();
            var products = _productService.GetProductByName(name);
            foreach (Product item in products)
            {
                var prodImg = _context.ProductImages.Where(x => x.ProductID == item.Id).First();
                if (prodImg != null)
                    list.Add(prodImg);
            }
            return list.Distinct().ToList();
        }
    }
}
