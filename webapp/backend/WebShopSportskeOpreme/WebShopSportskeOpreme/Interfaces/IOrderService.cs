using WebShopSportskeOpreme.Models;
using Stripe.Checkout;

namespace WebShopSportskeOpreme.Interfaces
{
    public interface IOrderService 
    {
        Order GetOrderById(int orderId);
        List<Order> GetOrdersByUserId(int userId);
        List<Order> GetAllOrders();
        DateTime GetOrderDate(int orderId);
        List<OrderItem> GetOrderItems(int orderId);
        bool CreateNewOrder(Order order);
        bool UpdateOrder(int id,Order order);
        bool DeleteOrder(int id);

        Task CreateOrderFromSession(Session session);

        bool UpdateOrderState(int orderId, OrderState newState);
    }
}
