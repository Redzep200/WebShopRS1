namespace WebShopSportskeOpreme.Models
{
    public class Coupon
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public int PercentDiscount { get; set; }
        public bool IsActive { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
