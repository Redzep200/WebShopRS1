using WebShopSportskeOpreme.Models;

namespace WebShopSportskeOpreme.Interfaces
{
    public interface IShoppingCartService
    {
        ShoppingCart GetShoppingCartById(int id);
        ShoppingCart GetShoppingCartByUserId(int userId);
        List<ShoppingCart> GetAllShoppingCarts();
        DateTime GetSCCreationDate(int id);
        bool CreateShoppingCart(ShoppingCart shoppingCart);
        bool UpdateShoppingCart(int id,ShoppingCart shoppingCart);
        bool DeleteShoppingCart(int id);
    }
}
