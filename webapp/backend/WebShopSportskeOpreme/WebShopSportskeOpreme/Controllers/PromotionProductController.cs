using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebShopSportskeOpreme.Interfaces;
using WebShopSportskeOpreme.Viewmodels;

namespace WebShopSportskeOpreme.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PromotionProductController : ControllerBase
    {
        private readonly IPromotionProductService _promotionProductService;
        public PromotionProductController(IPromotionProductService promotionProductService)
        {
            _promotionProductService = promotionProductService;
        }

        [HttpGet]
        public IActionResult GetAllPromotionProducts() 
        {
            var PromProd = _promotionProductService.GetAllPromotionProducts();
            return Ok(PromProd);
        }

        [HttpGet]
        public IActionResult GetPromotionProductById(int id)
        {
            var PromProd = _promotionProductService.GetPromotionProductById(id);
            if(PromProd == null) { return NotFound("Nije pronađeno"); }
            return Ok(PromProd);
        }

        [HttpPost]
        public IActionResult CreatePromotionProduct(AddPromotionsProductsVM model) 
        {
            var PromProd = _promotionProductService.CreatePromotionProduct(
                new Models.PromotionProduct { PromotionId = model.PromotionID, ProductId = model.ProductID });
            if(PromProd== false) { return BadRequest("Greška pri dodavanju"); }
            return Ok(new { success = true, message = "Uspješno dodavanje." });
        }

        [HttpDelete]
        public IActionResult DeletePromotionProduct(int PromotionID) 
        {
            var status = _promotionProductService.DeletePromotionProduct(PromotionID);
            if(status==false) { return NotFound("Greška pri brisanju"); }
            return Ok("Uspješno obrisano");
        }

        [HttpPut]
        public IActionResult UpdatePromotionProduct(int id, int productID, int promotionid)
        {
            var status = _promotionProductService.UpdatePromotionProduct(id, new Models.PromotionProduct { ProductId= productID,PromotionId= promotionid });
            if(status==false) { return NotFound("Greška pri ažuriranju"); }
            return Ok("Uspješno ažurirano");
        }
    }
}
