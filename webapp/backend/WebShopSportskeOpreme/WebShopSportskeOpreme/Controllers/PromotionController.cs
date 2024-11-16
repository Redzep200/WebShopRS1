using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Runtime.CompilerServices;
using WebShopSportskeOpreme.Interfaces;
using WebShopSportskeOpreme.Models;
using WebShopSportskeOpreme.Viewmodels;

namespace WebShopSportskeOpreme.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PromotionController : ControllerBase
    {
        private readonly IPromotionService _promotionService;

        public PromotionController(IPromotionService promotionService)
        {
            _promotionService = promotionService;
        }
        [HttpGet]
        public IActionResult GetAllPromotions() 
        {
            var promotions = _promotionService.GetAllPromotions();
            return Ok(promotions);
        }

        [HttpGet]
        public IActionResult GetPromoGetPromotionById(int id) 
        {
            var promotion = _promotionService.GetPromotionById(id);
            if(promotion==null)
                return NotFound("Promocija nije pronadjena");
            return Ok(promotion);
        }

        [HttpGet]
        public IActionResult GetPromotionByName(string name) 
        {
            var _promotion = _promotionService.GetPromotionByName(name);
            if (_promotion == null) { return NotFound("Promocija nije pronadjena"); }
            return Ok(_promotion);
        }

        [HttpGet]
        public IActionResult GetPromotionDate(int id) 
        {
            var _date = _promotionService.GetDateById(id);
            return Ok(_date);
        }

        [Authorize(Roles = "Web shop admin, SistemAdmin")]
        [HttpPost]
        public IActionResult AddPromotion(AddPromotionVM model) 
        {
            if (ModelState.IsValid)
            {
                var product = _promotionService.CreatePromotion(new Promotion
                {
                    Name = model.Name,
                    Description = model.Description,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    DiscountPercentage = model.DiscountPercentage
                });

                if (!product)
                    return BadRequest("Greška u dodavanju promocije!");

                return Ok(new { success = true, message = "Uspješno dodana promocija" });
            }

            return BadRequest(ModelState);
        }

        [Authorize(Roles = "Web shop admin, SistemAdmin")]
        [HttpDelete]
        public IActionResult DeletePromotion(int id) 
        {
            var status = _promotionService.DeletePromotion(id);
            if(status==false) { return NotFound("Neuspješno brisanje"); }
            return Ok("Uspješno obrisana promocija!");
        }

        [Authorize(Roles = "Web shop admin, SistemAdmin")]
        [HttpPut]
        public IActionResult UpdatePromotion( int id, string name, string description, DateTime startDate, DateTime endDate) 
        {
            var status = _promotionService.UpdatePromotion(id, new Models.Promotion { Name = name,Description = description,StartDate = startDate,EndDate = endDate });
            if(status==false) { return NotFound("Promocija nije pronadjena!"); }
            return Ok("Uspješno ažurirana promocija!");
        }
    }
}
