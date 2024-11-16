using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebShopSportskeOpreme.Interfaces;
using WebShopSportskeOpreme.Viewmodels;

namespace WebShopSportskeOpreme.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StoreImageController : ControllerBase
    {
        private readonly IStoreImageService _storeImageService;

        public StoreImageController(IStoreImageService storeImageService)
        {
            _storeImageService = storeImageService;
        }
        [HttpGet]
        public IActionResult GetByStoreID(int storeId)
        {
            var obj = _storeImageService.GetImagesByStoreId(storeId);
            if (obj == null)
                return NotFound("Odabrana prodavnica nema sliku!");
            return Ok(obj);

        }

        [HttpGet]
        public IActionResult GetAllStoreImages()
        {
            var obj = _storeImageService.GetAllStoreImages();
            if (obj == null)
                return NotFound();
            return Ok(obj);
        }


        [HttpGet]
        public IActionResult GetStoreImagesByStireName(string name)
        {
            var obj = _storeImageService.GetStoreImagesByStoreName(name);
            if (obj == null)
                return NotFound();
            return Ok(obj);
        }

        [HttpPost]
        public IActionResult AddStoreImage([FromBody] AddStoreImageVM storeImageVM)
        {
            var status = _storeImageService.CreateStoreImage(storeImageVM.StoreId,  storeImageVM.Image);
            if (!status)
            {
                return BadRequest(new { message = "Neuspjesno dodana slika za prodavnicu!" });
            }

            return Ok(new { message = "Uspjesno dodana slika!" });
        }

        [HttpDelete]
        public IActionResult RemoveStoreImage(int storeId)
        {
            var status = _storeImageService.DeleteStoreImageByStoreId(storeId);
            if (status == false)
                return BadRequest("Neuspjesno brisanje slike!");
            return Ok("Uspjesno obrisana slika!");
        }
    }
}
