using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebShopSportskeOpreme.Interfaces;
using WebShopSportskeOpreme.Models;
using WebShopSportskeOpreme.Viewmodels;

namespace WebShopSportskeOpreme.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [HttpGet]   
        public IActionResult GetCategoryById(int id)
        {
            var category = _categoryService.GetCategoryById(id);
            if(category == null)
            {
                return NotFound("Kategorija nije pronađena!");
            }
            return Ok(category);
        }
        [HttpGet]
        public IActionResult GetCategoryByName(string name)
        {
            var category = _categoryService.GetCategoryByName(name);
            if (category == null)
            {
                return NotFound("Kategorija nije pronađena!");
            }
            return Ok(category);
        }

        [HttpGet]
        public IActionResult GetAllCategories()
        {
            var categories = _categoryService.GetAllCategories();
            return Ok(categories);
        }
        [Authorize(Roles = "Web shop admin, SistemAdmin")]
        [HttpPost]
        public IActionResult AddCategory(AddCategoryVM model)
        {
            if (ModelState.IsValid)
            {
                var category = _categoryService.CreateCategory(new Category
                {
                    Name = model.Name,
                    Description = model.Description
                });

                if (!category)
                    return BadRequest("Greška u dodavanju kategorije!");

                return Ok(new { success = true, message = "Uspješno dodana kategorija" });
            }

            return BadRequest(ModelState);
        }
        [Authorize(Roles = "Web shop admin, SistemAdmin")]
        [HttpDelete]
        public IActionResult DeleteCategory(int id)
        {
            var category = _categoryService.DeleteCategory(id);
            if (category == false)
                return NotFound("Država sa ID" + id + "ne postoji!");
            return Ok(new { success = true, message = "Uspjesno obrisana kategorija" });
        }

        [Authorize(Roles = "Web shop admin, SistemAdmin")]
        [HttpPut]
        public IActionResult UpdateCategory(int id,string name, string description)
        {
            var status = _categoryService.UpdateCategory(id, new Category { Name = name, Description = description });
            if (status == false)
                return NotFound("Kategorija nije pronađena!");
            return Ok(new { success = true, message = "Uspjesno editovana kategorija" });
        }

    }
}
