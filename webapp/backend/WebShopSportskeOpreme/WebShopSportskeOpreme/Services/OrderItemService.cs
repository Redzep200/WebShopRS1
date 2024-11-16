using Microsoft.AspNetCore.Mvc.Razor;
using System.Data.SqlTypes;
using WebShopSportskeOpreme.Interfaces;
using WebShopSportskeOpreme.Models;

namespace WebShopSportskeOpreme.Services
{
    public class OrderItemService : IOrderItemService
    {
        public readonly WebShopDbContext _context;
        public readonly IOrderService _orderService;
        public readonly IProductService _productService;

        public OrderItemService(WebShopDbContext context, IOrderService orderService, IProductService productService)
        {
            _context = context;
            _orderService = orderService;
            _productService = productService;
        }

        public bool CreateNewOrderItem(OrderItem item)
        {
            if(item == null || _orderService.GetOrderById(item.OrderId)==null || _productService.GetProductById(item.ProductId)==null) 
                { return false; }
            _context.OrderItems.Add(item);
            _context.SaveChanges();
            return true;
        }

        public bool DeleteOrderItem(int id)
        {
            var Helper = _context.OrderItems.FirstOrDefault(x => x.Id == id);
            if (Helper == null) { return false;}
            _context.OrderItems.Remove(Helper);
            _context.SaveChanges();
            return true;
        }

        public List<OrderItem> GetAllOrders()
        {
            var Helper = _context.OrderItems.ToList();
            foreach (var item in Helper) { item.Order = _orderService.GetOrderById(item.OrderId);
                                           item.Product = _productService.GetProductById(item.ProductId);
            }
            return Helper;
        }

        public OrderItem GetOrderItem(int id)
        {
            var helper = _context.OrderItems.FirstOrDefault(x=>x.Id==id);
            if (helper != null)
            {
                helper.Order = _orderService.GetOrderById(helper.OrderId);
                helper.Product = _productService.GetProductById(helper.ProductId);
            }
            return helper;
        }

        public List<OrderItem> GetOrderItemsByOrderId(int id)
        {
            var Helper = _context.OrderItems.Where(x=> x.OrderId==id).ToList();
            foreach (var item in Helper)
            {
                item.Order = _orderService.GetOrderById(item.OrderId);
                item.Product = _productService.GetProductById(item.ProductId);
            }
            return Helper;
        }

        public decimal GetProductPrice(int id)
        {
            decimal Helper = _productService.GetProductById(id).Price;
            return Helper;
        }

        public decimal GetTotalPrice(int id , int qty)
        {
           var helper = _productService.GetProductById(id).Price;
            decimal TotalPrice = helper * qty;
            if(helper==null) { return 0; }
            return TotalPrice;
        }

        public bool UpdateOrderItemQuantity(int id, OrderItem item)
        {
            var Helper = _context.OrderItems.FirstOrDefault(item=>item.Id==id);
            if(Helper==null || item==null) 
                { return false; }
            Helper.Quantity = item.Quantity;
            Helper.Price = item.Price;
            Helper.TotalPrice = item.Quantity * item.Price;
            _context.OrderItems.Update(Helper);
            _context.SaveChanges();
            return true;
        }
    }
}
