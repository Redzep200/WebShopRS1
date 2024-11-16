using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebShopSportskeOpreme.Interfaces;

namespace WebShopSportskeOpreme.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public IActionResult GetAllOrders() 
        {
            var orders = _orderService.GetAllOrders();
            if(orders==null) return NotFound("Trenutno ne postoji ni jedna narudžba");
            return Ok(orders);
        }

        [HttpGet]
        public IActionResult GetOrderById(int id) 
        {
            var Helper = _orderService.GetOrderById(id);
            if (Helper == null) return NotFound("Ne postoji ni jedna narudžba koja ima taj ID");
            return Ok(Helper);
        }

        [HttpGet]
        public IActionResult GetOrdersByUserId(int userId)
        {
            var Helper = _orderService.GetOrdersByUserId(userId);
            if (Helper == null) return NotFound("Ne postoji ni jedna narudžba koja ima taj user id");
            return Ok(Helper);
        }

        [HttpPost]
        public IActionResult CreateNewOrder(int cartId) 
        {
            var NewOrder = _orderService.CreateNewOrder(new Models.Order { ShoppingCartId = cartId ,OrderPrice=0, OrderDate=DateTime.Now});
            if (NewOrder == false) return BadRequest("Greška pri kreaciji narudžbe");
            return Ok("Uspješno kreirana narudžba");
        }

        [HttpDelete]
        public IActionResult DeleteOrder(int id) 
        {
            var DeletedOrder = _orderService.DeleteOrder(id);
            if (DeletedOrder == false) return NotFound("Pokušavate obrisati nepostojeću narudžbu");
            return Ok("Narudžba uspješno obrisana");
        }

        [HttpPut]
        public IActionResult UpdateOrder(int orderid) 
        {
            var Items = _orderService.GetOrderItems(orderid);
            decimal TotalValue = 0;
            foreach(var item in Items)
            {
                TotalValue += item.Price;
            }
            var UpdatedOrder = _orderService.UpdateOrder(orderid, new Models.Order { OrderPrice= TotalValue });
            if (UpdatedOrder == false) return NotFound("Nepostojeća narudžba");
            return Ok("Uspješno ažurirana cijena narudžbe");
        }

        [HttpPut("{id}/state")]
        public IActionResult UpdateOrderState(int id, [FromBody] OrderStateUpdateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var success = _orderService.UpdateOrderState(id, model.NewState);

            return Ok(success);
        }
    }
}
