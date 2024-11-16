using WebShopSportskeOpreme.Interfaces;
using WebShopSportskeOpreme.Models;
using Stripe.Checkout;
namespace WebShopSportskeOpreme.Services
{
    public class OrderService : IOrderService
    {
        private readonly WebShopDbContext _context;
        private readonly IShoppingCartService _shopCartService;
        private readonly IShoppingCartItemService _shopItemService;
        private readonly ISmsService _smsService;
    
 

        public OrderService(WebShopDbContext context, IShoppingCartService shopCartService, IShoppingCartItemService shopItemService, ISmsService smsService)
        {
            _context = context;
            _shopCartService = shopCartService;
            _shopItemService = shopItemService;           
            _smsService = smsService;
        }

        public bool CreateNewOrder(Order order)
        {
            if(order == null || _shopCartService.GetShoppingCartById(order.ShoppingCartId)==null) return false;
            _context.Orders.Add(order); 
            _context.SaveChanges(); 
            return true;
        }

        public bool DeleteOrder(int id)
        {
            var Helper = _context.Orders.FirstOrDefault(o => o.Id == id);
            if(Helper == null) return false;
            _context.Orders.Remove(Helper);
            _context.SaveChanges();
            return true;
        }

        public List<Order> GetAllOrders()
        {
            var Helper = _context.Orders.ToList();
            foreach(var order in Helper) 
            { order.ShoppingCart = _shopCartService.GetShoppingCartById(order.ShoppingCartId); }
            return Helper;
        }

        public Order GetOrderById(int orderId)
        {
            var helper = _context.Orders.FirstOrDefault(o=>o.Id== orderId);
            if (helper != null) 
                helper.ShoppingCart = _shopCartService.GetShoppingCartById(helper.ShoppingCartId);
            return helper;
        }

        public List<Order> GetOrdersByUserId(int userId)
        {
            var helper = _context.Orders.Where(o => o.ShoppingCart.UserId == userId).ToList();
            foreach (var order in helper)
            { order.ShoppingCart = _shopCartService.GetShoppingCartById(order.ShoppingCartId); }
            return helper;
        }

        public DateTime GetOrderDate(int orderId)
        {
            DateTime Date = _context.Orders.FirstOrDefault(d=>d.Id== orderId).OrderDate;
            return Date;
        }

        public List<OrderItem> GetOrderItems(int orderId)
        {
            var ItemList = _context.OrderItems.Where(l=>l.OrderId==orderId).ToList();
            return ItemList;
        }
        public async Task CreateOrderFromSession(Session session)
        {
            var order = new Order
            {
                OrderDate = DateTime.Now,
                ShoppingCartId = int.Parse(session.Metadata["ShoppingCartId"]),
                OrderPrice = (decimal)session.AmountTotal / 100,
                ModifiedOrderDate = DateTime.Now,
                State = OrderState.Pending,
                PaymentIntentId = session.PaymentIntentId
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

           
            var shoppingCartId = int.Parse(session.Metadata["ShoppingCartId"]);
            var cartItems = _context.ShoppingCartItems
                .Where(ci => ci.ShoppingCartId == shoppingCartId)
                .ToList();
         
            foreach (var item in cartItems)
            {
                var orderItem = new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.ItemPrice,
                    TotalPrice = item.Quantity * item.ItemPrice
                };

                _context.OrderItems.Add(orderItem);
            }
            
            await _context.SaveChangesAsync();

            var userId = _context.ShoppingCarts
                        .Where(sc => sc.Id == shoppingCartId)
                        .Select(sc => sc.UserId)
                        .FirstOrDefault();

            if (userId > 0)
            {
                _shopItemService.DeleteAllShoppingCartItems(userId);
            }
        }

        public bool UpdateOrder(int id, Order order)
        {
            var UpdatedOrder = _context.Orders.FirstOrDefault(o=>o.Id==id);
            var ListOfItems = GetOrderItems(id);
            decimal totalPrice = 0;
            foreach (var item in ListOfItems)
            {
                 totalPrice +=item.TotalPrice;
            }
            if (UpdatedOrder == null || order == null ) return false;
            UpdatedOrder.OrderPrice = totalPrice;
            _context.Orders.Update(UpdatedOrder);
            _context.SaveChanges();
            return true;
        }
        public bool UpdateOrderState(int orderId, OrderState newState)
        {
            var order = _context.Orders.FirstOrDefault(o => o.Id == orderId);
            if (order == null)
            {
                return false;
            }
            order.State = newState;
            order.ModifiedOrderDate = DateTime.Now;
            _context.SaveChanges();

            _smsService.SendSmsAsync("38761316891", $"Your order status has been updated to: {newState}. Order ID: {orderId}." );

            return true;
        }      
    }


}
