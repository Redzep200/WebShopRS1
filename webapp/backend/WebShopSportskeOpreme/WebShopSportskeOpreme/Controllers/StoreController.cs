using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quartz;
using WebShopSportskeOpreme.Interfaces;
using WebShopSportskeOpreme.Models;
using WebShopSportskeOpreme.DTOs;

namespace WebShopSportskeOpreme.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        private readonly IStoreService _storeService;
        private readonly ILogger<StoreController> _logger;

        public StoreController(IStoreService storeService, ILogger<StoreController> logger)
        {
            _storeService = storeService;
            _logger = logger;
        }

        [HttpGet("GetAllStores")]
        public ActionResult<List<StoreDTO>> GetAllStores()
        {
            try
            {
                var stores = _storeService.GetAllStores();
                return Ok(stores);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri preuzimanju prodavnica");
                return StatusCode(500, "An error occurred while processing your request. Please try again later.");
            }
        }

        [HttpGet("GetStoreById/{id}")]
        public IActionResult GetStoreById(int id)
        {
            var store = _storeService.GetStoreById(id);
            if (store == null)
                return NotFound("Prodavnica nije nađena");
            return Ok(store);
        }

        [Authorize(Roles = "SistemAdmin")]
        [HttpPost("CreateStore")]
        public IActionResult CreateStore([FromBody] Store store)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = _storeService.CreateStore(store);
            if (!result)
                return BadRequest("Greška u dodavanju prodavnice");

            return Ok(new { success = true, message = "Uspješno dodana prodavnica" });
        }

        [Authorize(Roles = "SistemAdmin")]
        [HttpPut("UpdateStore/{id}")]
        public IActionResult UpdateStore(int id, [FromBody] Store store)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = _storeService.UpdateStore(id, store);
            if (!result)
                return NotFound("Greška pri izmjeni prodavnice");

            return Ok(new { success = true, message = "Prodavnica uspješno ažurirana" });
        }

        [Authorize(Roles = "SistemAdmin")]
        [HttpDelete("DeleteStore/{id}")]
        public IActionResult DeleteStore(int id)
        {
            var result = _storeService.DeleteStore(id);
            if (!result)
                return NotFound("Greška pri brisanju");

            return Ok(new { success = true, message = "Prodavnica uspješno obrisana" });
        }
    }
}