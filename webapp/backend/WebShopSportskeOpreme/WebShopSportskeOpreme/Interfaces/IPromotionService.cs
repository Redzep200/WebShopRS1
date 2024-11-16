using WebShopSportskeOpreme.Models;

namespace WebShopSportskeOpreme.Interfaces
{
    public interface IPromotionService
    {
        Promotion GetPromotionByName(string Name);
        Promotion GetPromotionById(int Id);
        DateTime GetDateById(int id);
        List<Promotion> GetAllPromotions();
        bool CreatePromotion(Promotion promotion);
        bool UpdatePromotion(int id,Promotion promotion);
        bool DeletePromotion(int id);
    }
}
