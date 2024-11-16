namespace WebShopSportskeOpreme.Models
{
   
    public class Notification
    {
        public int Id { get; set; }
        public int PromotionId { get; set; }
        public Promotion? Promotion { get; set; }
        public string Text { get; set; }
        public DateTime? NotificationDate { get; set; }
        public DateTime NotificationUpdateTime =new DateTime();
    }
}
