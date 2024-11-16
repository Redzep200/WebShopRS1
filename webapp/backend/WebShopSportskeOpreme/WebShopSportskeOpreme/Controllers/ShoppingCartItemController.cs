using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Xml;
using WebShopSportskeOpreme.Interfaces;
using WebShopSportskeOpreme.Models;
using WebShopSportskeOpreme.Services;
using WebShopSportskeOpreme.Viewmodels;

namespace WebShopSportskeOpreme.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ShoppingCartItemController : ControllerBase
    {
        private readonly IShoppingCartItemService _shoppingCartItemService;
        public ShoppingCartItemController(IShoppingCartItemService shoppingCartItemService)
        {
            _shoppingCartItemService = shoppingCartItemService;
        }

        [HttpGet]
        public IActionResult GetShoppingCartItembyId(int id) 
        {
            var SCI = _shoppingCartItemService.GetShoppingCartItemById(id);
            if(SCI==null) { return NotFound("Ta stavka ne postoji"); }
            return Ok(SCI);
        }

        [HttpGet]
        public IActionResult GetAllShoppingCartItems()
        {
            var SCI = _shoppingCartItemService.GetAllShoppingCartItems();
            if (SCI == null) { return NotFound("Trenutno ne postoji ni jedna stavka"); }
            return Ok(SCI);
        }

        [HttpPost]
        public IActionResult CreateNewShoppingCartItem(AddShoppingCartItemVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var price = _shoppingCartItemService.GetProductPrice(model.ProductId);
            var SCITotalPrice = _shoppingCartItemService.GetTotalPrice(model.ProductId, model.Quantity);

            var newShoppingCartItem = new Models.ShoppingCartItem
            {
                ProductId = model.ProductId,
                ShoppingCartId = model.ShoppingCartId,
                Quantity = model.Quantity,
                ItemPrice = price,
                TotalItemPrice = SCITotalPrice
            };

            var success = _shoppingCartItemService.CreateNewShoppingCartItem(newShoppingCartItem);

            if (!success)
            {
                return BadRequest("Neuspješno dodana stavka");
            }

            return Ok(new { success = true, message = "Uspješno dodana stavka" });
        }
        [HttpDelete]
        public IActionResult DeleteShopCartItem(int id)
        {
            var DeletedSCI = _shoppingCartItemService.DeleteShoppingCartItem(id);
            if(DeletedSCI==false) { return NotFound("Pokušavate obrisati nepostojeću stavku"); }
            return Ok(new { success = true, message = "Uspješno dodana stavka" });
        }
        [HttpDelete]
        public IActionResult DeleteAllShoppingCartItems(int userId)
        {
            var result = _shoppingCartItemService.DeleteAllShoppingCartItems(userId);
            if (result)
            {
                return Ok(new { message = "Uspješno obrisana korpa" });
            }
            return NotFound(new { message = "Nema artikala unutar korpe" });
        }
        [HttpPut]
        public IActionResult UpdateShoppingCartItem(int id, int qty)
        {
            var product = _shoppingCartItemService.GetShoppingCartItemById(id);
            var price = _shoppingCartItemService.GetProductPrice(product.ProductId);
            var UpdatedSCI = _shoppingCartItemService.UpdateShoppingCartItem(id, new Models.ShoppingCartItem { ItemPrice= price, Quantity=qty , TotalItemPrice=price*qty});
            if(UpdatedSCI==false) { return NotFound("Stavka nije pronađena"); }
            return Ok("Stavka uspješno ažurirana");
        }
    }
}
