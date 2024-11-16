using WebShopSportskeOpreme.Interfaces;
using WebShopSportskeOpreme.Models;

namespace WebShopSportskeOpreme.Services
{
    public class NotificationService : INotificationService
    {
        private readonly WebShopDbContext _context;
        private readonly IPromotionService _promotionService;

        public NotificationService(WebShopDbContext context, IPromotionService promotionService)
        {
            _context = context;
            _promotionService = promotionService;
        }

        public bool CreateNotification(Notification notification) 
        {
            if (notification == null || _promotionService.GetPromotionById(notification.PromotionId)==null) return false;
            _context.Notifications.Add(notification);
            _context.SaveChanges();
            return true;
        }

        public bool UpdateNotification(int id,Notification notification) 
        {
            var not = _context.Notifications.FirstOrDefault(o => o.Id == id);
            if(not == null || notification == null) return false;
            not.Text = notification.Text;
            not.NotificationUpdateTime = DateTime.Now;
            _context.Notifications.Update(not);
            _context.SaveChanges();
            return true;
        }

        public bool DeleteNotification(int id) 
        {
            var not = _context.Notifications.FirstOrDefault(o => o.Id == id);
            if(not == null) return false;
            _context.Notifications.Remove(not);
            _context.SaveChanges();
            return true;
        }

        public Notification GetNotificationById(int id) 
        {
            var not = _context.Notifications.FirstOrDefault(n=>n.Id==id);
            if (not != null)
                not.Promotion = _promotionService.GetPromotionById(not.PromotionId);
            return not;
        }

        public List<Notification> GetAllNotifications() 
        {
            var not = _context.Notifications.ToList();
            foreach (var item in not)
                item.Promotion = _promotionService.GetPromotionById(item.PromotionId);
            return not;
        }

        public DateTime GetPromotionStartDate(int id) 
        {
            var helper = _context.Promotions.FirstOrDefault(p => p.Id == id);
            var helperDate = helper.StartDate;
            return helperDate;
        }

        
    }
}
