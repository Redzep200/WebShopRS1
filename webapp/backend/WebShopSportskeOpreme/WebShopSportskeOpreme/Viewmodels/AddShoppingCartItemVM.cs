using System.ComponentModel.DataAnnotations;

namespace WebShopSportskeOpreme.Viewmodels
{
    public class AddShoppingCartItemVM
    {
        [Required]
        public int ProductId { get; set; }
        [Required]
        public int ShoppingCartId { get; set; }
        [Required]
        public int Quantity { get; set; }   
    }
}
