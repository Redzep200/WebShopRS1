using WebShopSportskeOpreme.Models;

namespace WebShopSportskeOpreme.Interfaces
{
    public interface IOrderItemService
    {
        OrderItem GetOrderItem(int id);
        List<OrderItem> GetAllOrders();
        List<OrderItem> GetOrderItemsByOrderId(int id);
        decimal GetTotalPrice(int id, int qty);
        decimal GetProductPrice(int id);
        bool CreateNewOrderItem(OrderItem item);
        bool UpdateOrderItemQuantity(int id, OrderItem item);
        bool DeleteOrderItem(int id);
    }
}
