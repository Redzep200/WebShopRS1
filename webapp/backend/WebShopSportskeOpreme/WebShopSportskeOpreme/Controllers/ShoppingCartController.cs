using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebShopSportskeOpreme.Interfaces;

namespace WebShopSportskeOpreme.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartService _shoppingCartService;

        public ShoppingCartController(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }

        [HttpGet]
        public IActionResult GetAllShoppingCarts() 
        {
            var Helper = _shoppingCartService.GetAllShoppingCarts();
            if(Helper == null) { return BadRequest("Trenutno ne postoji ni jedna aktivna korpa"); }
            return Ok(Helper);
        }

        [HttpGet]
        public IActionResult GetShoppingCartById(int id) 
        {
            var Helper = _shoppingCartService.GetShoppingCartById(id);
            if(Helper == null) { return NotFound("Korpa pod tim ID-em ne postoji"); }
            return Ok(Helper);
        }

        [HttpGet]
        public IActionResult GetShoppingCartByUserId(int id)
        {
            var Helper = _shoppingCartService.GetShoppingCartByUserId(id);
            if (Helper == null) { return NotFound("Korpa pod tim ID-em ne postoji"); }
            return Ok(Helper);
        }

        [HttpPost]
        public IActionResult CreateNewShoppingCart(int userid) 
        {
            var helper = _shoppingCartService.CreateShoppingCart(new Models.ShoppingCart { CreationDate= DateTime.Now , UserId=userid });
            if(helper==false) { return BadRequest("Neuspješno dodana korpa"); }
            return Ok("Uspjesno kreirana korpa");
        }

        [HttpDelete]
        public IActionResult DeleteShoppingCart(int id) 
        {
            var helper = _shoppingCartService.DeleteShoppingCart(id);
            if(helper==false) { return NotFound("Korpa pod tim Id-em ne postoji"); }
            return Ok("Korpa uspješno obrisana");
        }

        [HttpPut]
        public IActionResult UpdateShoppingCart(int id, Models.ShoppingCart model) 
        {
            DateTime DateHelper = _shoppingCartService.GetSCCreationDate(id);
            var helper = _shoppingCartService.UpdateShoppingCart(id, new Models.ShoppingCart { CreationDate = DateHelper, LastModifiedDate = DateTime.Now });
            if(helper==false) { return NotFound("Korpa ne postoji"); }
            return Ok("Korpa ažurirana");
        }
    }
}
