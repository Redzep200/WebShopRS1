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
    public class CityController : ControllerBase
    {
        private readonly WebShopDbContext _context;
        private readonly ICityService _cityService;
        public CityController(WebShopDbContext context, ICityService cityService)
        {
            _context = context;
            _cityService = cityService;
        }

        [HttpGet]
        public IActionResult GetAllCities()
        {
            var cities = _cityService.GetAllCities();
            return Ok(cities);
        }

        [HttpGet]
        public IActionResult GetCityById(int id)
        {
            var city = _cityService.GetCityById(id);
            if (city == null)
                return NotFound("Grad nije pronađen!");
            return Ok(city);
        }
        [Authorize(Roles = "Web shop admin, SistemAdmin")]
        [HttpPost]
        public IActionResult CreateCity(AddCityVM cityViewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            var city = new City
            {
                CountryId = cityViewModel.CountryId,
                Name = cityViewModel.Name,
                ZipCode = cityViewModel.ZipCode
            };

            var createdCity = _cityService.CreateCity(city);
            if (createdCity == null)
                return BadRequest("Greška u dodavanju grada!");

            return Ok(new { success = true, message = "Uspjesno dodan grad", city = createdCity });
        }

        [Authorize(Roles = "Web shop admin, SistemAdmin")]
        [HttpDelete]
        public IActionResult DeleteCity(int id)
        {
            var status = _cityService.DeleteCity(id);
            if (status == false)
                return NotFound("Grad nije pronađen!");
            return Ok(new { success = true, message = "Uspjesno obrisan grad" });
        }

        [Authorize(Roles = "Web shop admin, SistemAdmin")]
        [HttpPut]
        public IActionResult UpdateCity(int id, int countryid, string name, string zipcode)
        {
            var status = _cityService.UpdateCity(id, new City{ 
              CountryId = countryid, 
              Name = name, 
              ZipCode = zipcode });
            if (status == false)
                return NotFound("Grad nije pronađen!");
            return Ok(new { success = true, message = "Uspjesno editovan grad" });
        }
    }
}
