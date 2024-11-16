using Microsoft.EntityFrameworkCore;
using WebShopSportskeOpreme.Helpers;
using WebShopSportskeOpreme.Interfaces;
using WebShopSportskeOpreme.Models;
using WebShopSportskeOpreme.Viewmodels;

namespace WebShopSportskeOpreme.Services
{
    public class ProductService : IProductService
    {
        private readonly WebShopDbContext _context;
        private readonly ICategoryService _categoryService;
        private readonly IManufacturerService _manufacturerService;

        public ProductService(WebShopDbContext context, ICategoryService categoryService, IManufacturerService manufacturerService)
        {
            _context = context;
            _categoryService = categoryService;
            _manufacturerService = manufacturerService;
        }

        public bool CreateProduct(Product product)
        {
            if (product == null || _categoryService.GetCategoryById(product.CategoryId) == null ||
                _manufacturerService.GetManufacturerById(product.ManufacturerId) == null) return false;
            product.IsDeleted = false;
            _context.Products.Add(product);
            _context.SaveChanges();
            return true;

        }

        public bool DeleteProduct(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product == null) return false;
            if (!product.IsDeleted)
            {
                product.IsDeleted = true;
                product.DeletionDate = DateTime.Now;
                _context.Update(product);
                _context.SaveChanges();
            }
            return true;
        }

        public List<ProductItemVM> GetAllProductItems()
        {
            var list = new List<ProductItemVM>();
            var products = _context.Products.ToList();
            foreach (var item in products)
            {
                var productImg = _context.ProductImages.FirstOrDefault(x => x.ProductID == item.Id);
                var imageString = productImg != null
                    ? productImg.ImageFormat + ImageHelper.ToBase64(productImg.ImageByteArray)
                    : string.Empty;

                var obj = new ProductItemVM()
                {
                    ProductId = item.Id,
                    CategoryId = item.CategoryId,
                    ManufacturerId = item.ManufacturerId,
                    Name = item.Name,
                    Description = item.Description,
                    Price = item.Price,
                    ImageString = imageString
                };
                list.Add(obj);
            }
            return list;
        }

        public bool DeleteProductItem(int productId)
        {         
            var product = _context.Products.FirstOrDefault(p => p.Id == productId);
                   
            var productImage = _context.ProductImages.FirstOrDefault(pi => pi.ProductID == productId);

            if (productImage != null)
            {               
                _context.ProductImages.Remove(productImage);
            }
           
            _context.Products.Remove(product);
            _context.SaveChanges();

            return true;
        }

        public List<ProductItemVM> GetProductItemsByProductName(string productName)
        {
            var list = new List<ProductItemVM>();
            var prodItems = GetAllProductItems();
            list = prodItems.Where(x => x.Name.ToLower().Contains(productName.ToLower())).ToList();
            return list;
        }

        public List<Product> GetAllProducts()
        {
            var products = _context.Products.ToList();
            foreach (var item in products)
            {
                item.Category = _categoryService.GetCategoryById(item.CategoryId);
                item.Manufacturer = _manufacturerService.GetManufacturerById(item.ManufacturerId);
            }
            return products;
        }

        

        public List<Product> GetProductByCategory(int id)
        {
            var product = _context.Products.Where(p => p.CategoryId == id).ToList();
            return product;
        }

        public Product GetProductById(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
                if (product != null) {
                product.Category = _categoryService.GetCategoryById(product.CategoryId);
                product.Manufacturer = _manufacturerService.GetManufacturerById(product.ManufacturerId);
            }
            return product;
        }

        public List<Product> GetProductByName(string productName)
        {
            var product = _context.Products.Where(p => p.Name.ToLower().Contains(productName.ToLower())).ToList();
            return product;
        }

        public List<ProductItemVM> GetProductItemsByCategoryId(int categoryId)
        {
            var list = new List<ProductItemVM>();
            var prodItems = GetAllProductItems();
            list = prodItems.Where(p => p.CategoryId == categoryId).ToList();
            return list;
        }

        public ProductItemVM GetProductItemByProductId(int productId)
        {
            var prodItems = GetAllProductItems();
            var item = prodItems.FirstOrDefault(p => p.ProductId == productId);
            return item;
        }

        public bool UpdateProduct(int id, Product product)
        {
            var updatedProduct  = GetProductById(id);
            if (product == null || updatedProduct == null || _categoryService.GetCategoryById(product.CategoryId) == null ||
                _manufacturerService.GetManufacturerById(product.ManufacturerId) == null)
                return false;
            updatedProduct.CategoryId = product.CategoryId;
            updatedProduct.Name = product.Name;
            updatedProduct.Description = product.Description;
            updatedProduct.Price = product.Price;
            updatedProduct.ManufacturerId = product.ManufacturerId;
            _context.Products.Update(updatedProduct);
            _context.SaveChanges();
            return true;
        }

        public List<ProductItemVM> GetProductItemsByPriceRange(int minPrice, int maxPrice)
        {
            var list = new List<ProductItemVM>();
            var prodItems = GetAllProductItems();
            list = prodItems.Where(x => x.Price >= minPrice && x.Price <= maxPrice).ToList();
            return list;
        }

        public List<ProductItemVM> GetProductItemsByManufacturerId(int manufacturerId)
        {
            var list = new List<ProductItemVM>();
            var prodItems = GetAllProductItems();
            list = prodItems.Where(x => x.ManufacturerId == manufacturerId).ToList();
            return list;
        }
        public List<ProductItemVM> GetFilteredProductItems(string? productName, int? categoryId, int? minPrice, int? maxPrice, int? manufacturerId)
        {
            if (minPrice.HasValue && maxPrice.HasValue && minPrice > maxPrice)
            {
                throw new ArgumentException("minPrice cannot be greater than maxPrice");
            }

            var prodItems = GetAllProductItems().AsQueryable();

            if (!string.IsNullOrEmpty(productName))
            {
                prodItems = prodItems.Where(x => x.Name.ToLower().Contains(productName.Trim().ToLower()));
            }

            if (categoryId.HasValue)
            {
                prodItems = prodItems.Where(p => p.CategoryId == categoryId.Value);
            }

            if (minPrice.HasValue)
            {
                prodItems = prodItems.Where(x => x.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                prodItems = prodItems.Where(x => x.Price <= maxPrice.Value);
            }

            if (manufacturerId.HasValue)
            {
                prodItems = prodItems.Where(x => x.ManufacturerId == manufacturerId.Value);
            }

            return prodItems.ToList();
        }

    }
}
