using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebShopSportskeOpreme.Interfaces;
using WebShopSportskeOpreme.Models;

namespace WebShopSportskeOpreme.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrderItemController : ControllerBase
    {
        private readonly IOrderItemService _orderItemService;

        public OrderItemController(IOrderItemService orderItemService)
        {
            _orderItemService = orderItemService;
        }

        [HttpGet]
        public IActionResult GetAllOrderItems() 
        {
            var OI = _orderItemService.GetAllOrders();
            if(OI == null) { return NotFound("Ne postoji ni jedna stavka"); }
            return Ok(OI);
        }

        [HttpGet]
        public IActionResult GetOrderItemsByOrderId(int id)
        {
            var OI = _orderItemService.GetOrderItemsByOrderId(id);
            if (OI == null) { return NotFound("Ne postoji ni jedna stavka"); }
            return Ok(OI);
        }

        [HttpGet]
        public IActionResult GetOrderItemById(int id) 
        {
            var OI = _orderItemService.GetOrderItem(id);
            if(OI == null) { return NotFound("Ne postoji stavka sa tim ID"); }
            return Ok(OI);
        }

        [HttpPost]
        public IActionResult CreateOrderItem(int orderid, int productid, int qty) 
        {
            var OIPrice = _orderItemService.GetProductPrice(productid);
            var TotalPrice = _orderItemService.GetTotalPrice(productid,qty);
            var NewOI = _orderItemService.CreateNewOrderItem(new Models.OrderItem { OrderId = orderid, ProductId = productid, Quantity = qty, Price = OIPrice, TotalPrice = TotalPrice });
            if(NewOI==false) { return BadRequest("Greška pri kreaciji stavke"); }
            return Ok("Uspješno dodane stavke");
        }

        [HttpDelete]
        public IActionResult DeleteOrderItem(int id) 
        {
            var Helper = _orderItemService.DeleteOrderItem(id);
            if(Helper==false) { return NotFound("Nema stavka sa tim ID"); }
            return Ok("Stavka uspješno obrisana");
        }

        [HttpPut]
        public IActionResult UpdateOrderItem(int id, int qty) 
        {
            var Helper = _orderItemService.GetOrderItem(id);
            var OIPrice = _orderItemService.GetProductPrice(Helper.ProductId);
            var UpdatedOI = _orderItemService.UpdateOrderItemQuantity(id, new Models.OrderItem { Quantity = qty, Price = OIPrice, TotalPrice = OIPrice * qty });
            if(UpdatedOI==false) { return NotFound("Pokušavate ažurirati nepostojecu stavku"); }
            return Ok("Uspješno ažurirana stavka");
        }
    }
}
