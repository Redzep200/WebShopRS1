using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebShopSportskeOpreme.Interfaces;
using WebShopSportskeOpreme.Viewmodels;

namespace WebShopSportskeOpreme.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductImageController : ControllerBase
    {
        private readonly IProductImageService _productImageService;

        public ProductImageController(IProductImageService productImageService)
        {
            _productImageService = productImageService;
        }
        [HttpGet]
        public IActionResult GetByProductID(int productId)
        {
            var obj = _productImageService.GetImagesByProductId(productId);
            if(obj==null) 
                return NotFound("Odabrani proizvod nema slike!");
            return Ok(obj);

        }

        [HttpGet]
        public IActionResult GetAllProductImages()
        {
            var obj = _productImageService.GetAllProductImages();
            if (obj == null)
                return NotFound();
            return Ok(obj);
        }

        [HttpGet]
        public IActionResult GetProductImagesByCategoryId(int categoryId)
        {
            var obj = _productImageService.GetProductImagesByCategoryId(categoryId);
            if (obj == null)
                return NotFound();
            return Ok(obj);

        }

        [HttpGet]
        public IActionResult GetProductImagesByProductName(string name)
        {
            var obj = _productImageService.GetProductImagesByProductName(name);
            if (obj == null)
                return NotFound();
            return Ok(obj);
        }

        [Authorize(Roles = "Web shop admin, SistemAdmin")]
        [HttpPost]
        public IActionResult AddProductImage([FromBody] AddProjectImageVM projectImageVM)
        {
            var status = _productImageService.CreateProductImage(projectImageVM.ProductId, projectImageVM.Image);
            if (!status)
            {
                return BadRequest(new { message = "Neuspjesno dodana slika za proizvod!" });
            }

            return Ok(new { message = "Uspjesno dodana slika!" });
        }

        [Authorize(Roles = "Web shop admin, SistemAdmin")]
        [HttpDelete]
        public IActionResult RemoveProductImage(int productId)
        {
            var status = _productImageService.DeleteProductImageByProductId(productId);
            if (status == false)
                return BadRequest("Neuspjesno brisanje slike!");
            return Ok("Uspjesno obrisana slika!");
        }
    }
}
