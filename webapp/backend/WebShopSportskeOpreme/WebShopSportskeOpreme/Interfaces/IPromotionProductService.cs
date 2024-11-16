using WebShopSportskeOpreme.Models;

namespace WebShopSportskeOpreme.Interfaces
{
    public interface IPromotionProductService
    {
        PromotionProduct GetPromotionProductById(int id);
        List<PromotionProduct> GetAllPromotionProducts();
        bool CreatePromotionProduct(PromotionProduct promotionProduct);
        bool UpdatePromotionProduct(int id,PromotionProduct promotionProduct);
        bool DeletePromotionProduct(int id);
    }
}
