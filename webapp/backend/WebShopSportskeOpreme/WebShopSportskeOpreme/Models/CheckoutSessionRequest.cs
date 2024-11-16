namespace WebShopSportskeOpreme.Models
{
    public class CheckoutSessionRequest
    {
        public List<ShoppingCartItem> CartItems { get; set; }
        public string CouponCode { get; set; }
    }
}
