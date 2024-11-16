using WebShopSportskeOpreme.Models;

namespace WebShopSportskeOpreme.Interfaces
{
    public interface INotificationService
    {
        Notification GetNotificationById(int id);
        List<Notification> GetAllNotifications();
        DateTime GetPromotionStartDate(int id);
        bool CreateNotification(Notification notification);
        bool DeleteNotification(int id);
        bool UpdateNotification(int id,Notification notification);
    }
}
