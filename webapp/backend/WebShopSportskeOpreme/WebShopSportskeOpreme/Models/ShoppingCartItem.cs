using System.ComponentModel.DataAnnotations.Schema;
using WebShopSportskeOpreme.Viewmodels;

namespace WebShopSportskeOpreme.Models
{
    public class ShoppingCartItem
    {
        public int Id { get; set; }
        public ShoppingCart? ShoppingCart { get; set; }
        public int ShoppingCartId { get; set; }
        [NotMapped]
        public ProductItemVM? Product { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal ItemPrice { get; set; }
        public decimal TotalItemPrice { get; set; }
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
