using WebShopSportskeOpreme.Interfaces;
using WebShopSportskeOpreme.Models;

namespace WebShopSportskeOpreme.Services
{
    public class PromotionProductService : IPromotionProductService
    {
        private readonly WebShopDbContext _context;
        private readonly IProductService _productService;
        private readonly IPromotionService _promotionService;

        public PromotionProductService(WebShopDbContext context, IProductService productService, IPromotionService promotionService)
        {
            _context = context;
            _productService = productService;
            _promotionService = promotionService;
        }

        public bool CreatePromotionProduct(PromotionProduct promotionProduct)
        {
            var product = _productService.GetProductById(promotionProduct.ProductId);
            var promotion = _promotionService.GetPromotionById(promotionProduct.PromotionId);

            if (promotionProduct== null || _productService.GetProductById(promotionProduct.ProductId)==null 
                || _promotionService.GetPromotionById(promotionProduct.PromotionId)==null) return false;
            if (promotion.DiscountPercentage > 0)
            {              
                decimal discountedPrice = product.Price - (product.Price * promotion.DiscountPercentage / 100);           
                product.Price = discountedPrice;
                _context.Products.Update(product);
            }
            _context.PromotionsProducts.Add(promotionProduct);
            _context.SaveChanges();
            return true;
        }

        public bool DeletePromotionProduct(int id)
        {
            var PromProd = _context.PromotionsProducts.FirstOrDefault(p => p.Id == id);
            if(PromProd==null) return false;
            _context.PromotionsProducts.Remove(PromProd);
            _context.SaveChanges();
            return true;
        }

        public List<PromotionProduct> GetAllPromotionProducts()
        {
            var PromProd = _context.PromotionsProducts.ToList();
            foreach(var item in PromProd)
            {
                item.Promotion = _promotionService.GetPromotionById(item.PromotionId);
                item.Product = _productService.GetProductById(item.ProductId);
            }
            return PromProd;
        }

        public PromotionProduct GetPromotionProductById(int id)
        {
            var PromProd = _context.PromotionsProducts.FirstOrDefault(p=>p.Id==id);
            if (PromProd != null)
            {
                PromProd.Promotion = _promotionService.GetPromotionById(PromProd.PromotionId);
                PromProd.Product = _productService.GetProductById(PromProd.ProductId);
            }
            return PromProd;
        }

        public bool UpdatePromotionProduct(int id, PromotionProduct promotionProduct)
        {
            var newPromProd = _context.PromotionsProducts.FirstOrDefault(p=>p.Id==id);
            if(newPromProd == null || promotionProduct == null || _promotionService.GetPromotionById(promotionProduct.PromotionId)==null
                || _productService.GetProductById(promotionProduct.ProductId) == null) { return false; }
            newPromProd.ProductId = promotionProduct.ProductId;
            newPromProd.PromotionId= promotionProduct.PromotionId;
            newPromProd.Product = _productService.GetProductById(promotionProduct.ProductId);
            newPromProd.Promotion = _promotionService.GetPromotionById(promotionProduct.PromotionId);
            _context.PromotionsProducts.Update(newPromProd);
            _context.SaveChanges();
            return true;
        }
    }
}
