using WebShopSportskeOpreme.Models;

namespace WebShopSportskeOpreme.Interfaces
{
    public interface IShoppingCartItemService
    {
        ShoppingCartItem GetShoppingCartItemById(int id);
        List<ShoppingCartItem> GetAllShoppingCartItems();
        decimal GetTotalPrice(int id, int qty);
        decimal GetProductPrice(int id);
        bool CreateNewShoppingCartItem(ShoppingCartItem shoppingCartItem);
        bool DeleteShoppingCartItem(int id);
        bool DeleteAllShoppingCartItems(int userId);
        bool UpdateShoppingCartItem(int id, ShoppingCartItem shoppingCartItem);
    }
}
