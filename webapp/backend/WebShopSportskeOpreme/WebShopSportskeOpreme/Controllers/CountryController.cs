using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebShopSportskeOpreme.Interfaces;
using WebShopSportskeOpreme.Models;
using WebShopSportskeOpreme.Services;
using WebShopSportskeOpreme.Viewmodels;

namespace WebShopSportskeOpreme.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly ICountryService _countryService;
        public CountryController(WebShopDbContext context, ICountryService countryService)
        {
            _countryService = countryService;
        }
        [HttpGet]
        public IActionResult GetAllCountries()
        {
            var countries = _countryService.GetAllCountries();
            return Ok(countries);
        }
        [HttpGet]
        public IActionResult GetCountryById(int id)
        {
            var country = _countryService.GetCountryById(id);
            if (country == null)
                return NotFound("Država nije pronađena!");
            return Ok(country);
        }

        [Authorize(Roles = "Web shop admin, SistemAdmin")]
        [HttpPost]
        public IActionResult AddCountry(AddCountryVM model)
        {
            if (ModelState.IsValid)
            {
                var country = _countryService.CreateCountry(new Country
                {
                    Name = model.Name
                });

                var createdCountry = _countryService.CreateCountry(country);
                if (createdCountry == null)
                    return BadRequest("Greška u dodavanju države!");

                return Ok(new { success = true, message = "Uspješno dodana država" });

            }
            return BadRequest(ModelState);
        }

        [Authorize(Roles = "Web shop admin, SistemAdmin")]
        [HttpDelete]
        public IActionResult DeleteCountry(int id)
        {
            var status = _countryService.DeleteCountry(id);
            if(status == false)
                return NotFound("Država sa ID " + id + " ne postoji!");
            return Ok(new { success = true, message = "Uspjesno obrisana drzava" });
        }

        [Authorize(Roles = "Web shop admin, SistemAdmin")]
        [HttpPut]
        public IActionResult UpdateCountry(int id, string name)
        {
            var status = _countryService.UpdateCountry(id,new Country { Name = name });
            if (status == false)
                return NotFound("Država nije pronađena!");
            return Ok(new { success = true, message = "Uspjesno editovana drzava" });
        }

        [HttpGet("GetCountryByName")]
        public IActionResult GetCountryByName(string name)
        {
            var country = _countryService.GetCountryByName(name);
            if (country == null)
                return NotFound("Država nije pronađena");
            return Ok(country);
        }

        [HttpGet]
        public IActionResult GetOrCreateCountryByName(string name)
        {
            var country = _countryService.GetOrCreateCountryByName(name);
            return Ok(country);
        }
    }
}
