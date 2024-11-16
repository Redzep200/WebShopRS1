namespace WebShopSportskeOpreme.Models
{
    public class CartItem
    {
        public string Name { get; set; }
        public long UnitAmount { get; set; }
        public int Quantity { get; set; }

        public static CartItem FromShoppingCartItem(ShoppingCartItem item)
        {
            return new CartItem
            {
                Name = item.Product?.Name ?? "Unknown Product",
                UnitAmount = (long)(item.ItemPrice * 100),
                Quantity = item.Quantity
            };
        }
    }
}