using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WebShopSportskeOpreme.Interfaces;
using WebShopSportskeOpreme.Models;
using WebShopSportskeOpreme.Viewmodels;

namespace WebShopSportskeOpreme.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly WebShopDbContext _context;
        private readonly IProductService _productService;

        public ProductController(WebShopDbContext context, IProductService productService)
        {
            _context = context;
            _productService = productService;   
        }
        [HttpGet]
        public IActionResult GetAllProducts() {
        var products = _productService.GetAllProducts();
            return Ok(products);
        }

        [HttpGet]
        public IActionResult GetAllProductItems()
        {
            var productItems = _productService.GetAllProductItems();
            return Ok(productItems);
        }

        [HttpGet]
        public IActionResult GetProductItemsByCategoryId(int categoryId)
        {
            var prodItems = _productService.GetProductItemsByCategoryId(categoryId);
            return Ok(prodItems);
        }
        [HttpGet]
        public IActionResult GetProductItemsByProductName(string productName)
        {
            var prodItems = _productService.GetProductItemsByProductName(productName);
            return Ok(prodItems);
        }
        [HttpGet]
        public IActionResult GetProductItemsByPriceRange(int minPrice, int maxPrice)
        {
            var prodItems = _productService.GetProductItemsByPriceRange(minPrice, maxPrice);
            return Ok(prodItems);
        }
        [HttpGet]
        public IActionResult GetProductItemsByManufacturerId(int manufacturerId)
        {
            var prodItems = _productService.GetProductItemsByManufacturerId(manufacturerId);
            return Ok(prodItems);
        }

        [HttpGet]
        public IActionResult GetProductById(int id) 
        {
            var product = _productService.GetProductById(id);
            if(product == null) { return NotFound("Proizvod koji tražite ne postoji!"); }
            return Ok(product);
        }
        [HttpGet]

        public IActionResult GetProductByCategory(int id)
        {
            var product = _productService.GetProductByCategory(id);
            if (product == null) { return NotFound("Nema dostupnih proizvoda u ovoj kategoriji"); }
            return Ok(product);
        }

        [HttpGet]
        public IActionResult GetProductByName(string name)
        {
            var products = _productService.GetProductByName(name);
            if (products == null) { return NotFound("Nema dostupnih proizvoda u ovoj kategoriji"); }
            return Ok(products);
        }

        [Authorize(Roles = "Web shop admin, SistemAdmin")]
        [HttpPost]
        public IActionResult CreateProduct(AddProductVM model)
        {
            if (ModelState.IsValid)
            {
                var product =  _productService.CreateProduct(new Product
                {
                 CategoryId = model.CategoryId,
                 ManufacturerId = model.ManufacturerId,
                 Name = model.Name,
                 Description = model.Description,
                 Price = model.Price
                });

                if (!product)
                    return BadRequest("Greška u dodavanju proizvoda!");

                return Ok(new { success = true, message = "Uspješno dodan proizvoda" });
            }

            return BadRequest(ModelState);
        }

        [Authorize(Roles = "Web shop admin, SistemAdmin")]
        [HttpDelete]
        public IActionResult DeleteProduct(int id)
        {
            var status = _productService.DeleteProduct(id);
            if (status == false) return BadRequest("Neuspješno brisanje proizvoda!");
            return Ok(new { success = true, message = "Uspjesno obrisan proizvod" });
        }

        [Authorize(Roles = "Web shop admin, SistemAdmin")]
        [HttpDelete]     
        public IActionResult DeleteProductItem(int id)
        {
            var result = _productService.DeleteProductItem(id);

            if (result)
            {
                return Ok(new { message = "Uspješno obrisan proizvod!" });
            }
            else
            {
                return NotFound(new { message = "Neuspješno brisanje proizvoda!" });
            }
        }

        [Authorize(Roles = "Web shop admin, SistemAdmin")]
        [HttpPut]
        public IActionResult UpdateProduct(int id, int categoryId, int manufacturerId, string name, string description, decimal price)
        {
            var status = _productService.UpdateProduct(id, new Models.Product
            {
                CategoryId = categoryId,
                ManufacturerId = manufacturerId,
                Name = name,
                Description = description,
                Price = price
            });
            if (status == false)
                return NotFound("Proizvod ne postoji!");
            return Ok(new { success = true, message = "Uspjesno editovan proizvod" });
        }
        
        [HttpGet]
        public IActionResult GetProductImagesByProductId(int productId)
        {
            var productImages = _productService.GetProductItemByProductId(productId);
           
            return Ok(productImages);
        }

        [HttpGet]
        public IActionResult GetFilteredProductItems(
       [FromQuery] string? productName = "",
       [FromQuery] int? categoryId = null,
       [FromQuery][Range(0, int.MaxValue)] int? minPrice = null,
       [FromQuery][Range(0, int.MaxValue)] int? maxPrice = null,
       [FromQuery] int? manufacturerId = null)
        {
            try
            {
                var prodItems = _productService.GetFilteredProductItems(productName, categoryId, minPrice, maxPrice, manufacturerId);
                return Ok(prodItems);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
