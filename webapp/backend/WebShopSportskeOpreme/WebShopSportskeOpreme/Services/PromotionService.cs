using WebShopSportskeOpreme.Interfaces;
using WebShopSportskeOpreme.Models;

namespace WebShopSportskeOpreme.Services
{
    public class PromotionService : IPromotionService
    {
        private readonly WebShopDbContext _context;

        public PromotionService(WebShopDbContext context)
        {
            _context = context;
        }
        public bool CreatePromotion(Promotion promotion)
        {
           if(promotion == null) { return false; }
           _context.Promotions.Add(promotion);
            _context.SaveChanges();
            return true;
        }

        public bool DeletePromotion(int id)
        {
            var promotion = _context.Promotions.FirstOrDefault(p => p.Id == id);
            if(promotion == null) { return false; }
            _context.Promotions.Remove(promotion);
            _context.SaveChanges();
            return true;
        }

        public List<Promotion> GetAllPromotions()
        {
            var promotions = _context.Promotions.ToList();
            return promotions;
        }

        public DateTime GetDateById(int id)
        {
            var Date = _context.Promotions.FirstOrDefault(d => d.Id == id).StartDate;
            return Date;
        }

        public Promotion GetPromotionById(int Id)
        {
            var promotion = _context.Promotions.FirstOrDefault(p=>p.Id==Id);
            return promotion;
        }

        public Promotion GetPromotionByName(string Name)
        {
            var promotion = _context.Promotions.FirstOrDefault(p=>p.Name ==Name);
            return promotion;
        }

        public bool UpdatePromotion(int id, Promotion promotion)
        {
            var _promotion = _context.Promotions.FirstOrDefault(p=>p.Id== id);
            if (promotion==null || _promotion==null) { return false; }
            _promotion.Name = promotion.Name;
            _promotion.Description = promotion.Description;
            _promotion.StartDate = promotion.StartDate;
            _promotion.EndDate = promotion.EndDate;
            _context.Promotions.Update(_promotion);
            _context.SaveChanges();
            return true;
        }
    }
}
