namespace WebShopSportskeOpreme.Models
{
    public enum OrderState
    {
        Pending,
        Processing,
        Shipped,
        Delivered,
        Cancelled,
        Refunded
    }
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public ShoppingCart? ShoppingCart { get; set; }
        public int ShoppingCartId { get; set; }
        public DateTime ModifiedOrderDate = new DateTime();
        public decimal OrderPrice { get; set; }
        public OrderState State { get; set; }
        public string? PaymentIntentId { get; set; }
    }
}
